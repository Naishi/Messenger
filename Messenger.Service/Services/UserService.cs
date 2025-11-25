using AutoMapper;

using Messenger.Domain;
using Messenger.Domain.Entities;
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

    public UserService(IUserRepository userRepository, IEmailValidator emailValidator,
        IMapper mapper, IProfanityFilter profanityFilter)
    {
        _userRepository = userRepository;
        _emailValidator = emailValidator;
        _mapper = mapper;
        _profanityFilter = profanityFilter;
    }

    public async Task DeleteUserAsync(string userEmail)
    {
        if (!_emailValidator.IsEmailSyntaxValid(userEmail))
            throw new InvalidEmailException();

        if (!await _userRepository.DeleteUserAsync(userEmail))
            throw new UserNotFoundException();
    }

    public async Task<UserModel?> FindUserAsync(string searchRequest, SearchType searchType)
    {
        var userEntity = await _userRepository.FindUserByCriteriaAsync(searchRequest, searchType);
        if (userEntity == null)
            return null;
        var userModel = _mapper.Map<UserModel>(userEntity);
        return userModel;
    }

    public async Task UpdateUserAsync(UserModel userModel, string searchRequest, SearchType searchType)
    {
        var userEntity = await _userRepository.FindUserByCriteriaAsync(searchRequest, searchType);
        if (userEntity == null)
            throw new UserNotFoundException();

        if (!string.IsNullOrWhiteSpace(userModel.Name))
            userEntity.Name = userModel.Name;
        else
        {
            throw new EmptyStringsException();
        }
        if (!string.IsNullOrWhiteSpace(userModel.NickName))
            userEntity.NickName = userModel.NickName;
        else
        {
            throw new EmptyStringsException();
        }
        if (!string.IsNullOrWhiteSpace(userModel.Description))
            userEntity.Description = userModel.Description;
        else
        {
            throw new EmptyStringsException();
        }

        if (_profanityFilter.ContainsProfanity(userModel.Description)
            || _profanityFilter.ContainsProfanity(userModel.Name)
            || _profanityFilter.ContainsProfanity(userModel.NickName))
        {
            throw new ProfanityExistException();
        }

        await _userRepository.UpdateUserInfoAsync();
    }

    public async Task<List<UserModel>?> GetUsersAsync()
    {
        var userList = await _userRepository.GetUsersAsync();
        if (userList == null)
            return null;
        return _mapper.Map<List<UserModel>>(userList);
    }

    public async Task AddContactAsync(int ownerUserId, int contactUserId, string displayName)
    {
        var ownerUserEntity = await _userRepository.FindUserByIdAsync(ownerUserId);
        var contactUserEntity = await _userRepository.FindUserByIdAsync(contactUserId);
        if (contactUserEntity == null || ownerUserId == contactUserId)
            throw new UserNotFoundException();

        var ownerContactEntity = new ContactEntity
        {
            OwnerUserId = ownerUserId,
            ContactUserId = contactUserId,
            DisplayName = displayName
        };
        var contactContactEntity = new ContactEntity
        {
            OwnerUserId = contactUserId,
            ContactUserId = ownerUserId,
            DisplayName = ownerUserEntity.NickName ?? ownerUserEntity.Name
        };
        var checkContact = await _userRepository.FindContactByIdAsync(ownerUserId, contactUserId);
        if (checkContact != null)
            throw new ExistedUserException();

        await _userRepository.AddContactAsync(ownerContactEntity);
        await _userRepository.AddContactAsync(contactContactEntity);
    }

    public async Task DeleteContactAsync(int ownerId, int contactId)
    {
        var contactEntity = await _userRepository.GetContactAsync(contactId);
        var ownerContactEntity = await _userRepository.FindUserByIdAsync(ownerId);
        if (contactEntity == null || ownerContactEntity == null)
            throw new UserNotFoundException();

        if (!await _userRepository.RoleCheckAsync(ownerContactEntity) || contactEntity.OwnerUserId != ownerId)
            throw new ImproperUserException();

        await _userRepository.DeleteContactAsync(contactEntity);
    }
}