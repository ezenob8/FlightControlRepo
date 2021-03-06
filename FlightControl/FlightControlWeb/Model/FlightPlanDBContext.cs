﻿using Microsoft.EntityFrameworkCore;

namespace FlightControlWeb.Model
{
    public class FlightPlanDBContext : DbContext
    {
        public DbSet<FlightPlan> FlightPlans { get; set; }
        public DbSet<Location> Segments { get; set; }
        public DbSet<Flight> Flight { get; set; }
        public DbSet<Server> Servers { get; set; }

        public FlightPlanDBContext(DbContextOptionsBuilder options)
        : base(options.Options)
        { }
        public FlightPlanDBContext() : this(new DbContextOptionsBuilder()
            // Sending to normal construcor with this defualt sql server 
            .UseSqlServer("Data Source=(local);Initial Catalog=FlightPlanModelDBMigrations;" +
            "user=sa;password=clave123;multipleactiveresultsets=True;"))
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MapInitLocationOfFlightPlan(modelBuilder);
            MapSegmentOfFlightPlan(modelBuilder);
            MapFlights(modelBuilder);
            MapServers(modelBuilder);
        }

        /// <summary>
        /// Configuration Mapping Server Entity, Entity Framework
        /// </summary>
        /// <param name="modelBuilder"></param>
        private static void MapServers(ModelBuilder modelBuilder)
        {
            #region Mapping Servers
            modelBuilder.Entity<Server>(ob =>
            {
                ob.ToTable("Servers");
                ob.Property(o => o.ServerURL).HasColumnName("ServerURL");
            });
            #endregion
        }
        /// <summary>
        /// Configuration Mapping Flight Entity, Entity Framework
        /// </summary>
        /// <param name="modelBuilder"></param>
        private static void MapFlights(ModelBuilder modelBuilder)
        {
            #region Mapping Flight
            modelBuilder.Entity<FlightPlan>()
            .HasOne<Flight>(s => s.Flight)
            .WithOne(ad => ad.FlightPlan)
            .HasForeignKey<Flight>(ad => ad.FlightPlanId);
            #endregion
        }

        /// <summary>
        /// Configuration Mapping Segments Entity, Entity Framework
        /// </summary>
        /// <param name="modelBuilder"></param>
        private static void MapSegmentOfFlightPlan(ModelBuilder modelBuilder)
        {
            #region Mapping Segments of FlightPlan
            modelBuilder.Entity<Location>()
            .HasOne<FlightPlan>(s => s.FlightPlan)
            .WithMany(g => g.Segments)
            .HasForeignKey(s => s.FlightPlanId);
            #endregion
        }

        /// <summary>
        /// Configuration Mapping InitLocation Entity, Entity Framework
        /// </summary>
        /// <param name="modelBuilder"></param>
        private static void MapInitLocationOfFlightPlan(ModelBuilder modelBuilder)
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
        }
    }
}