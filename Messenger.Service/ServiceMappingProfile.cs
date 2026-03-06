using AutoMapper;
using Messenger.Domain.Entities;
using Messenger.Service.Models;

namespace Messenger.Service;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        CreateMap<ChatModel, ChatEntity>().ReverseMap();
        CreateMap<UserAuthModel, UserAuthEntity>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ReverseMap();

        CreateMap<UserAuthRegisterModel, UserAuthEntity>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        CreateMap<UserAuthLoginModel, UserAuthModel>();
        CreateMap<UserModel, UserEntity>().ReverseMap();
        CreateMap<MessageModel, MessageEntity>().ReverseMap();
        CreateMap<UserChatModel, UserChatEntity>().ReverseMap();
    }
}