using FluentValidation;

namespace MeuCiclo.Application.Commands
{
    public class CreateCicloCommandValidator : AbstractValidator<CreateCicloCommand>
    {
        public CreateCicloCommandValidator()
        {
            RuleFor(x => x.Data)
                .NotEmpty();

            RuleFor(x => x.Fluxo)
                .NotEmpty()
                .Must(x => new[] { "Leve", "Moderado", "Intenso" }.Contains(x));
        }
    }
}
