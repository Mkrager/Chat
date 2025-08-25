using AutoMapper;
using Chat.Application.Features.Chat.Commands.PostMessage;
using Chat.Application.Features.Chat.Queries.GetMessageList;
using Chat.Domain.Entities;

namespace Chat.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Message, PostMessageCommand>().ReverseMap();
            CreateMap<Message, PostMessageResponse>().ReverseMap();
            CreateMap<Message, MessageListVm>().ReverseMap();
        }
    }
}
