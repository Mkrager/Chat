using FluentValidation;

namespace Chat.Application.Features.Chat.Commands.PostMessage
{
    public class PostMessageCommandValidator : AbstractValidator<PostMessageCommand>
    {
        public PostMessageCommandValidator()
        {
            RuleFor(p => p.Iv)
                .NotEmpty().WithMessage("Message can't be empty")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Message can't contain only spaces");
            
            RuleFor(p => p.Ciphertext)
                .NotEmpty().WithMessage("Message can't be empty")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Message can't contain only spaces");

            RuleFor(p => p.ReceiverId)
                .NotEmpty().WithMessage("Message can't be empty");
        }
    }
}
