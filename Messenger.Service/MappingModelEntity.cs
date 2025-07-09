using AutoMapper;
using Messenger.Domain.Entities;
using Messenger.Service.Models;

namespace Messenger.Service;

public class MappingModelEntity : Profile
{
    public MappingModelEntity()
    {
        CreateMap<ChatModel, ChatEntity>()
            .ForMember
            (dest => dest.Id,
                opt => opt.Ignore())
            .ForMember
            (dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember
            (dest => dest.CreatedDate,
                opt => opt.MapFrom(src => src.CreatedDate))
            .ReverseMap()
            .ForMember
            (dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember
            (dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember
            (dest => dest.CreatedDate,
                opt => opt.MapFrom(src => src.CreatedDate));

        CreateMap<UserAuthModel, UserAuthEntity>()
            .ForMember
                (dest => dest.Id, opt => opt.Ignore())
            .ForMember
                (dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember
                (dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
            .ReverseMap()
            .ForMember
                (dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember
                (dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember
                (dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash));
    }
}