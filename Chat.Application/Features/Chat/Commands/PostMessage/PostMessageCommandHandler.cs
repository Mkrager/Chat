using AutoMapper;
using Chat.Domain.Entities;
using MediatR;
using Chat.Application.Contracts.Persistance;

namespace Chat.Application.Features.Chat.Commands.PostMessage
{
    public class PostMessageCommandHandler : IRequestHandler<PostMessageCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IChatRepository _chatRepository;

        public PostMessageCommandHandler(IMapper mapper, IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(PostMessageCommand request, CancellationToken cancellationToken)
        {
            var validator = new PostMessageCommandValidator();
            var validatorResult = await validator.ValidateAsync(request);

            if (validatorResult.Errors.Count > 0)
                throw new Exceptions.ValidationException(validatorResult);

            var message = _mapper.Map<Message>(request);

            message = await _chatRepository.AddAsync(message);

            return message.Id;
        }
    }
}
