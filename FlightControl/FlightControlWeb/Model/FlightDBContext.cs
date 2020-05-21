using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace FlightControlWeb.Model
{
    public class FlightDBContext : DbContext
    {
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Location> Segments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Data Source=(local);Initial Catalog=FlightModelDB;user=sa;password=clave123;multipleactiveresultsets=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Mapping InitialLocation of Flight
            modelBuilder.Entity<Flight>(ob =>
            {
                ob.ToTable("Flights");
                ob.Property(o => o.Passengers).HasColumnName("Passengers");
                ob.Property(o => o.CompanyName).HasColumnName("CompanyName");
                ob.HasOne(o => o.InitialLocation).WithOne()
                    .HasForeignKey<InitialLocation>(o => o.FlightId);
            });
            #endregion

            #region Mapping Segments of Flight
            modelBuilder.Entity<Location>()
            .HasOne<Flight>(s => s.Flight)
            .WithMany(g => g.Segments)
            .HasForeignKey(s => s.FlightId);
            #endregion


            #region Mapping Flight Plan
            modelBuilder.Entity<Flight>()
            .HasOne<FlightPlan>(s => s.FlightPlan)
            .WithOne(ad => ad.Flight)
            .HasForeignKey<FlightPlan>(ad => ad.FlightId);
            #endregion

        }
    }
}