
using AutoMapper;
using Chat.Application.Features.Account.Queries.Registration;
using Chat.Application.Features.Chat.Commands.PostMessage;
using Chat.Application.Features.Chat.Queries.GetMessageList;
using Chat.Application.Features.User.Queries.GetUserList;
using Chat.Domain.Entities;
using Chat.Domain.Models;

namespace Chat.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Message, PostMessageCommand>().ReverseMap();
            CreateMap<Message, MessageListVm>().ReverseMap();  
            
            CreateMap<User, UserListVm>().ReverseMap();
            CreateMap<User, MessageUserDto>().ReverseMap();

        }
    }
}
