using AutoMapper;
using Messenger.Dtos;
using Messenger.Dtos.ChatDtos;
using Messenger.Dtos.UserDtos;
using Messenger.Service.Models;
using Messenger.Service.Models.Enums;

namespace Messenger;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateChatDto, ChatModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ChatName))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(DateTime.Now)));

        CreateMap<UserAuthDto, UserAuthRegisterModel>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(_ => UserRole.Client));

        CreateMap<UserAuthDto, UserAuthLoginModel>();

        CreateMap<RegisterRequest, UserModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday))
            .ForMember(dest => dest.NickName, opt => opt.MapFrom(src => src.NickName))
            .ReverseMap();
        CreateMap<RegisterRequest, UserAuthRegisterModel>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ReverseMap();
        CreateMap<UserInfoDto, UserModel>().ReverseMap();
        CreateMap<UserChangingInfoDto, UserModel>().ReverseMap();
    }
}