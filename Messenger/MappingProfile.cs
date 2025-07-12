using AutoMapper;
using Messenger.Dtos;
using Messenger.Dtos.ChatDtos;
using Messenger.Service.Models;
using Messenger.Service.Models.Enums;

namespace Messenger;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateChatDto, ChatModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"src.ChatName"))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(DateTime.Now)));

        CreateMap<UserAuthDto, UserRegisterModel>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(_ => UserRole.Client));

        CreateMap<UserAuthDto, UserLoginModel>();
    }
}