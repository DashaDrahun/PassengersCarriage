using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.BL.Infrastructure;

namespace Lab06.MVC.Carriage.BL.Interfaces
{
    public interface IIdentityService : IDisposable
    {
        Task<OperationDetails> Create(UserModel userModel);
        Task<ClaimsIdentity> Authenticate(UserModel userModel);
    }
}
