using FluentValidation;

namespace Chat.Application.Features.Account.Command.Authentication
{
    public class AuthenticationQueryCommandValidator : AbstractValidator<AuthenticationCommand>
    {
        public AuthenticationQueryCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
