using FluentValidation;
using AccountApi.Entities;

namespace AccountApi.Models.Validators
{
    public class LoginUserValidator:AbstractValidator<LoginDto>
    {
        public LoginUserValidator(AccountDbContext db)
        {
            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = db.Users.Any(u => u.Email == value);
                    if (!emailInUse)
                    {
                        context.AddFailure("Email", "Account with this Email does not exist");
                    }
                });
        }
    }
}
