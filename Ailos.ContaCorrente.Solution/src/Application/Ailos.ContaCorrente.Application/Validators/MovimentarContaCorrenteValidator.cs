using Ailos.ContaCorrente.Application.Commands.MovimentarContaCorrente;
using FluentValidation;

namespace Ailos.ContaCorrente.Application.Validators;

public class MovimentarContaCorrenteValidator : AbstractValidator<MovimentarContaCorrenteCommand>
{
    public MovimentarContaCorrenteValidator()
    {
        RuleFor(x => x.IdRequisicao)
            .NotEmpty().WithMessage("ID da requisição é obrigatório");

        RuleFor(x => x.ContaLogadaId)
            .GreaterThan(0).WithMessage("ID da conta logada deve ser maior que zero");

        RuleFor(x => x.ContaCorrenteId)
            .GreaterThan(0).WithMessage("ID da conta deve ser maior que zero")
            .When(x => x.ContaCorrenteId.HasValue);

        RuleFor(x => x.Valor)
            .GreaterThan(0).WithMessage("Valor deve ser maior que zero");

        RuleFor(x => x.Tipo)
            .NotEmpty().WithMessage("Tipo de movimento é obrigatório")
            .Must(tipo => tipo.ToUpper() == "C" || tipo.ToUpper() == "D")
            .WithMessage("Tipo deve ser 'C' (Crédito) ou 'D' (Débito)");
    }
}
