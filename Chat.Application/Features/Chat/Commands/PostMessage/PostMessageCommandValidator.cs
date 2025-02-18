using FluentValidation;

namespace Chat.Application.Features.Chat.Commands.PostMessage
{
    public class PostMessageCommandValidator : AbstractValidator<PostMessageCommand>
    {
        public PostMessageCommandValidator()
        {
            RuleFor(p => p.Content)
                .NotEmpty()
                .NotNull()
                .WithMessage("Message can't be empty");
        }
    }
}
