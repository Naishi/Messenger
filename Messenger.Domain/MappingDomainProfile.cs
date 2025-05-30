using AutoMapper;
using Messenger.Domain.Entities;
using Messenger.Service.Models;


namespace Messenger.Domain;

public class MappingDomainProfile: Profile
{
    public MappingDomainProfile()
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
    }
}