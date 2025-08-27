using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Features.Account.Command.Registration;
using Chat.Application.Features.Account.Queries.Authentication;
using Chat.Application.Features.Chat.Commands.PostMessage;
using Chat.Application.Features.Chat.Queries.GetMessageList;
using Chat.Application.Features.Users.GetUserList;
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

            CreateMap<GetUserDetailsResponse, UserListVm>().ReverseMap();

            CreateMap<RegistrationRequest, RegistrationCommand>().ReverseMap();

            CreateMap<AuthenticationRequest, AuthenticationQuery>().ReverseMap();
            CreateMap<AuthenticationResponse, AuthenticationVm>().ReverseMap();
        }
    }
}
