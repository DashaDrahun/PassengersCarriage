using System.Web;
using Autofac;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.BL.Mappers;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.BL.Services;
using Lab06.MVC.Carriage.DAL.Context;
using Lab06.MVC.Carriage.DAL.Entities;
using Lab06.MVC.Carriage.DAL.Identity;
using Lab06.MVC.Carriage.DAL.Interfaces;
using Lab06.MVC.Carriage.DAL.Repositories;
using Lab06.MVC.Carriage.ModelBuilders;
using Microsoft.AspNet.Identity;

namespace Lab06.MVC.Carriage
{
    public class AutofacConfig: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PassengersContext>()
                .InstancePerRequest();

            builder.RegisterType<ApplicationUserManager>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUserStore>()
                .As<IUserStore<AppUser>>()
                .InstancePerLifetimeScope();

            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication)
                .InstancePerLifetimeScope();

            builder.RegisterType<RepositoryFactory>()
                .As<IRepositoryFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BaseRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<IdentityService>()
                .As<IIdentityService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AdminService>()
                .As<IAdminService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ModelBuilder>()
                .As<IModelBuilder>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RouteMapper>()
                .As<IWrapMapper<RouteModel, Route>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TripMapper>()
                .As<IWrapMapper<TripModel, Trip>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderMapper>()
                .As<IWrapMapper<OrderModel, Order>>()
                .InstancePerLifetimeScope();
        }
    }
}