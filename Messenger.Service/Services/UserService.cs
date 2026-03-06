using AutoMapper;
using Messenger.Domain;
using Messenger.Domain.Entities;
using Messenger.Domain.Filters;
using Messenger.Domain.Interfaces;
using Messenger.Service.Exceptions;
using Messenger.Service.Interfaces;
using Messenger.Service.Models;
using ProfanityFilter.Interfaces;

namespace Messenger.Service.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailValidator _emailValidator;
    private readonly IMapper _mapper;
    private readonly IProfanityFilter _profanityFilter;
    private readonly IContactRepository _contactRepository;

    public UserService(
        IUserRepository userRepository,
        IEmailValidator emailValidator,
        IMapper mapper,
        IProfanityFilter profanityFilter,
        IContactRepository contactRepository)
    {
        _userRepository = userRepository;
        _emailValidator = emailValidator;
        _mapper = mapper;
        _profanityFilter = profanityFilter;
        _contactRepository = contactRepository;
    }

    public async Task DeleteUserAsync(string userEmail)
    {
        if (!_emailValidator.IsEmailSyntaxValid(userEmail))
        {
            throw new InvalidEmailException();
        }

        var user = await _userRepository.GetAsync(
            new UserFilter()
            {
                Search = userEmail
            });

        if (user.Count == 0)
        {
            throw new UserNotFoundException();
        }

        await _userRepository.DeleteAsync(user[0]);
    }

    public async Task<UserModel?> FindUserAsync(string searchRequest, SearchType searchType)
    {
        var userEntity = await _userRepository.GetAsync(new UserFilter()
        {
            Search = searchRequest,
            SearchType = searchType
        });

        if (userEntity.Count == 0)
        {
            return null;
        }
        
        var userModel = _mapper.Map<UserModel>(userEntity);

        return userModel;
    }

    public async Task UpdateUserAsync(UserModel userModel)
    {
        var userEntity = await _userRepository.GetAsync(new UserFilter()
        {
            UserIds = [userModel.Id]
        });

        if (userEntity.Count == 0)
        {
            throw new UserNotFoundException();
        }

        if (string.IsNullOrEmpty(userModel.Name)
            && string.IsNullOrEmpty(userModel.NickName)
            && string.IsNullOrEmpty(userModel.Description))
        {
            throw new EmptyStringsException();
        }

        if (userEntity[0].Name == userModel.Name
            && userEntity[0].NickName == userModel.NickName
            && userEntity[0].Description == userModel.Description)
        {
            throw new EmptyStringsException("don't have changes");
        }

        if (_profanityFilter.ContainsProfanity(userModel.Description)
            || _profanityFilter.ContainsProfanity(userModel.Name)
            || _profanityFilter.ContainsProfanity(userModel.NickName))
        {
            throw new ProfanityExistException();
        }

        var modifiedUserEntity =  _mapper.Map<UserEntity>(userModel);
        await _userRepository.UpdateAsync(modifiedUserEntity);
    }

    public async Task AddContactAsync(int ownerUserId, int contactUserId, string displayName)
    {
        var userEntities = await _userRepository.GetAsync(
            new()
            {
                UserIds = [ownerUserId, contactUserId]
            });

        UserEntity ownerUserEntity, contactUserEntity;
        
        if (userEntities.Count == 2
            && userEntities[0].Id == ownerUserId)
        {
            ownerUserEntity = userEntities[0];
            contactUserEntity = userEntities[1];
        }
        else
        {
            ownerUserEntity = userEntities[1];
            contactUserEntity = userEntities[0];
        }

        if (contactUserEntity == null || ownerUserId == contactUserId)
        {
            throw new UserNotFoundException();
        }

        var ownerContactEntity = new ContactEntity
        {
            OwnerUserId = ownerUserId, ContactUserId = contactUserId, DisplayName = displayName
        };
        
        var contactContactEntity = new ContactEntity
        {
            OwnerUserId = contactUserId,
            ContactUserId = ownerUserId,
            DisplayName = ownerUserEntity.NickName ?? ownerUserEntity.Name
        };
        
        var checkContact = await _contactRepository.GetAsync(
            new()
            {
                OwnerUserId = ownerUserId, ContactUserId = contactUserId
            });

        if (checkContact != null)
        {
            throw new ExistedUserException();
        }

        await using var transaction = await _contactRepository.BeginTransactionAsync();

        try
        {
            await _contactRepository.CreateAsync(ownerContactEntity);
            await _contactRepository.CreateAsync(contactContactEntity);
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteContactAsync(int ownerId, int contactId)
    {
        var contactEntities = await _contactRepository.GetAsync(
            new()
            {
                OwnerUserId = ownerId, ContactUserId = contactId
            });

        if (contactEntities.Count == 0)
        {
            throw new UserNotFoundException();
        }

        if (contactEntities.Any(c => c.OwnerUserId == ownerId || c.OwnerUserId == contactId))
        {
            throw new ImproperUserException();
        }
        
        await using var transaction = await _contactRepository.BeginTransactionAsync();
        
        try
        {
            await _contactRepository.DeleteAsync(contactEntities[0]);
            await _contactRepository.DeleteAsync(contactEntities[1]);
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}