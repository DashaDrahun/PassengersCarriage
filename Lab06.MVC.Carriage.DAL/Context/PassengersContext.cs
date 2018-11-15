using System.Configuration;
using System.Data.Entity;
using Lab06.MVC.Carriage.DAL.Entities;
using Lab06.MVC.Carriage.DAL.Initializers;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lab06.MVC.Carriage.DAL.Context
{
    public class PassengersContext : IdentityDbContext
    {
        static PassengersContext()
        {
            Database.SetInitializer(new PassengersDbContextInitializer());
        }

        public PassengersContext()
            : base(ConfigurationManager.ConnectionStrings["Connectionlab06"].ConnectionString)
        {
            Database.Initialize(false);
        }

        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<Trip> Trips { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<AppUser> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().Property(p => p.FirstName).HasColumnType("NVARCHAR").HasMaxLength(50);
            modelBuilder.Entity<AppUser>().Property(p => p.LastName).HasColumnType("NVARCHAR").HasMaxLength(50);

            modelBuilder.Entity<Route>().HasKey(p => p.Id);
            modelBuilder.Entity<Route>().Property(p => p.CityArr).IsRequired();
            modelBuilder.Entity<Route>().Property(p => p.CityDepart).IsRequired();

            modelBuilder.Entity<Trip>().HasKey(p => p.Id);
            modelBuilder.Entity<Trip>().Property(p => p.Arrival).IsRequired();
            modelBuilder.Entity<Trip>().Property(p => p.Departure).IsRequired();
            modelBuilder.Entity<Trip>().Property(p => p.FreeSeetsNumbers).HasColumnType("xml").IsRequired();
            modelBuilder.Entity<Trip>().Property(p => p.Price).IsRequired();
            modelBuilder.Entity<Trip>().HasRequired(p => p.Route).WithMany(p => p.Trips).HasForeignKey(p => p.RouteId);

            modelBuilder.Entity<Order>().HasKey(p => p.Id);
            modelBuilder.Entity<Order>().Property(p => p.SeatNumber).IsRequired();
            modelBuilder.Entity<Order>().HasRequired(p => p.Trip).WithMany(p => p.Orders).HasForeignKey(p => p.TripId);
            modelBuilder.Entity<Order>().HasRequired(p => p.User).WithMany(p => p.Orders).HasForeignKey(p => p.UserId);
        }
    }
}
