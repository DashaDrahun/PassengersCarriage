using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lab06.MVC.Carriage.DAL.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
