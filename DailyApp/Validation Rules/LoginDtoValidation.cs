using DailyApp.DTOs;
using FluentValidation;

namespace DailyApp.Validation_Rules
{
    public class LoginDtoValidation : AbstractValidator<LoginDto>
    {
        public LoginDtoValidation()
        {

            RuleFor(x=>x.Email).NotEmpty().WithMessage("Email boş bırakılamaz"). Length(5,30).WithMessage("Email 5 ve 30 karakter arasında olmalıdır.");
            RuleFor(x=>x.Password). NotEmpty().WithMessage("Password boş bırakılamaz"). Length(6,10).WithMessage("Şifre 6 ile 10 karekter olmak zorundadır");
        }
    }
    }