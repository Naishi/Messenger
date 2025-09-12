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
    private readonly IMapper  _mapper;
    private readonly IProfanityFilter  _profanityFilter;
    
    public UserService(IUserRepository userRepository, IEmailValidator emailValidator,
        IMapper mapper, IProfanityFilter  profanityFilter)
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
            
        if(!await _userRepository.DeleteUserAsync(userEmail))
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
        
        if (_profanityFilter.ContainsProfanity(userModel.Description) 
            || _profanityFilter.ContainsProfanity(userModel.Name) 
            || _profanityFilter.ContainsProfanity(userModel.NickName))
        {
            throw new ProfanityExistException();
        }
        
        if (!string.IsNullOrWhiteSpace(userModel.Name))
            userEntity.Name = userModel.Name;
        if (!string.IsNullOrWhiteSpace(userModel.NickName))
            userEntity.NickName = userModel.NickName;
        if (!string.IsNullOrWhiteSpace(userModel.Description))
            userEntity.Description = userModel.Description;
        else
        {
            throw new EmptyStringChangesException();
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
}