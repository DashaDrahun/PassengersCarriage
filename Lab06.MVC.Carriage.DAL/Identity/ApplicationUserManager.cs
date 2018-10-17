using Lab06.MVC.Carriage.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace Lab06.MVC.Carriage.DAL.Identity
{
    public class ApplicationUserManager : UserManager<AppUser>
    {
        public ApplicationUserManager(IUserStore<AppUser> store)
            : base(store)
        {
            UserValidator = new UserValidator<AppUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6
            };
        }
    }
}
