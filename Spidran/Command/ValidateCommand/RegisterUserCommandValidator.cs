using FluentValidation;
using Spidran.Behaviors;

namespace Spidran.Command.ValidateCommand
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Некорректный формат email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен")
                .MinimumLength(6).WithMessage("Пароль должен содержать минимум 6 символов");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Имя обязательно");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Фамилия обязательна");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Дата рождения обязательна")
                .Must(BeAtLeast18YearsOld).WithMessage("Пользовать должен быть совершеннолетним лоол");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Телефон обязателен");

        }

        private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
        {
            return dateOfBirth <= DateTime.Today.AddYears(-18);
        }

    }
}
