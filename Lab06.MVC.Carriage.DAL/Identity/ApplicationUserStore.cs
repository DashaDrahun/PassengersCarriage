using Lab06.MVC.Carriage.DAL.Context;
using Lab06.MVC.Carriage.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lab06.MVC.Carriage.DAL.Identity
{
    public class ApplicationUserStore : UserStore<AppUser>
    {
        public ApplicationUserStore(PassengersContext context)
            : base(context)
        {
        }
    }
}
