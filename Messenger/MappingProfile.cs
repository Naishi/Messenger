using AutoMapper;
using Messenger.Dtos.ChatDtos;
using Messenger.Service.Models;

namespace Messenger;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateChatDto, ChatModel>()
            .ForMember
                (dest => dest.Name,
                    opt => opt.MapFrom(src => $"src.ChatName"))
            .ForMember
                (dest => dest.Id,
                    opt => opt.Ignore())
            .ForMember
                (dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(DateTime.Now)));
        
    }
}