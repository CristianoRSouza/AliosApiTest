using Ailos.ContaCorrente.Application.Commands.CadastrarContaCorrente;
using FluentValidation;

namespace Ailos.ContaCorrente.Application.Validators;

public class CadastrarContaCorrenteValidator : AbstractValidator<CadastrarContaCorrenteCommand>
{
    public CadastrarContaCorrenteValidator()
    {
        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("CPF é obrigatório")
            .Length(11).WithMessage("CPF deve ter 11 dígitos")
            .Matches(@"^\d{11}$").WithMessage("CPF deve conter apenas números");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MinimumLength(2).WithMessage("Nome deve ter pelo menos 2 caracteres")
            .MaximumLength(100).WithMessage("Nome não pode ter mais de 100 caracteres");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória")
            .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres");
    }
}
