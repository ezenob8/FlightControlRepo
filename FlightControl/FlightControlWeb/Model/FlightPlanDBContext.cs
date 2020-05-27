using System.Collections.Generic;
using FlightControlWeb.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightControlWeb.Model
{
    public class FlightPlanDBContext : DbContext
    {
        public DbSet<FlightPlan> FlightPlans { get; set; }
        public DbSet<Location> Segments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Data Source=(local);Initial Catalog=FlightPlanModelDB;user=sa;password=clave123;multipleactiveresultsets=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Mapping InitialLocation of FlightPlan
            modelBuilder.Entity<FlightPlan>(ob =>
            {
                ob.ToTable("FlightPlans");
                ob.Property(o => o.Passengers).HasColumnName("Passengers");
                ob.Property(o => o.CompanyName).HasColumnName("CompanyName");
                ob.HasOne(o => o.InitialLocation).WithOne()
                    .HasForeignKey<InitialLocation>(o => o.FlightPlanId);
            });
            #endregion

            #region Mapping Segments of FlightPlan
            modelBuilder.Entity<Location>()
            .HasOne<FlightPlan>(s => s.FlightPlan)
            .WithMany(g => g.Segments)
            .HasForeignKey(s => s.FlightPlanId);
            #endregion


            #region Mapping Flight
            modelBuilder.Entity<FlightPlan>()
            .HasOne<Flight>(s => s.Flight)
            .WithOne(ad => ad.FlightPlan)
            .HasForeignKey<Flight>(ad => ad.FlightPlanId);
            #endregion

        }
    }
}