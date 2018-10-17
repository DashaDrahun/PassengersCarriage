using Microsoft.AspNet.Identity.EntityFramework;

namespace Lab06.MVC.Carriage.DAL.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string name)
            : base(name)
        {
        }
    }
}
