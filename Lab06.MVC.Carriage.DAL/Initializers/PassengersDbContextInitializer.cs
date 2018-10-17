using System.Data.Entity;
using Lab06.MVC.Carriage.DAL.Context;
using Lab06.MVC.Carriage.DAL.Entities;
using Lab06.MVC.Carriage.DAL.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lab06.MVC.Carriage.DAL.Initializers
{
    public class PassengersDbContextInitializer: CreateDatabaseIfNotExists<PassengersContext>
    {
        protected override void Seed(PassengersContext context)
        {
            InitializeRoutes(context);
            InitializeUsers(context);
        }

        private void InitializeRoutes(PassengersContext context)
        {
            context.Routes.Add(new Route
            {
                CityArr = "Minsk",
                CityDepart = "Svetlogorsk",
                Kilometres = 222
            });
        }

        private void InitializeUsers(PassengersContext context)
        {
            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));
            var userManager = new ApplicationUserManager(new UserStore<AppUser>(context));

            var roleAdmin = new ApplicationRole { Name = "Admin" };
            var roleUser = new ApplicationRole { Name = "User" };

            roleManager.Create(roleAdmin);
            roleManager.Create(roleUser);

            var admin = new AppUser { Email = "admin@mail.ru", UserName = "Admin", FirstName = "Darya", LastName = "Drahun"};
            userManager.Create(admin, "qwerty");
            userManager.AddToRole(admin.Id, "Admin");

            base.Seed(context);
        }
    }
}
