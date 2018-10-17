using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.DAL.Entities;
using Lab06.MVC.Carriage.DAL.Identity;
using Lab06.MVC.Carriage.DAL.Interfaces;
using Microsoft.AspNet.Identity;

namespace Lab06.MVC.Carriage.BL.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly ApplicationUserManager userManager;
        private readonly IUnitOfWork unitOfWork;
        private bool disposed;

        public IdentityService(ApplicationUserManager userManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager
                               ?? throw new ArgumentNullException(nameof(userManager));
            this.unitOfWork = unitOfWork
                               ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<OperationDetails> Create(UserModel userModel)
        {
            AppUser user = await userManager.FindAsync(userModel.Name, userModel.Password);

            if (user == null)
            {
                user = new AppUser { Email = userModel.Email, UserName = userModel.Name };
                var result = await userManager.CreateAsync(user, userModel.Password);

                if (result.Errors.Any())
                {
                    return new OperationDetails(false, string.Join("", result.Errors.ToArray()), String.Empty);
                }

                await userManager.AddToRoleAsync(user.Id, userModel.Role);

                return new OperationDetails(true, "Register has success", String.Empty);
            }

            return new OperationDetails(false, "This user already exists", "Email");
        }

        public async Task<ClaimsIdentity> Authenticate(UserModel userModel)
        {
            ClaimsIdentity claim = null;
            AppUser user = await userManager.FindAsync(userModel.Name, userModel.Password);

            if (user != null && userManager.FindByEmail(userModel.Email) == user)
            {
                claim = await userManager.CreateIdentityAsync(
                    user,
                    DefaultAuthenticationTypes.ApplicationCookie);
            }

            return claim;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    unitOfWork.Dispose();
                }
                disposed = true;
            }
        }
    }
}
