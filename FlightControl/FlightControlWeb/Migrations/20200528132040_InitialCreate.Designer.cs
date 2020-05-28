﻿// <auto-generated />
using System;
using FlightControlWeb.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FlightControlWeb.Migrations
{
    [DbContext(typeof(FlightPlanDBContext))]
    [Migration("20200528132040_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FlightControlWeb.Model.Flight", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FlightIdentifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("FlightPlanId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsExternal")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("FlightPlanId")
                        .IsUnique();

                    b.ToTable("Flight");
                });

            modelBuilder.Entity("FlightControlWeb.Model.FlightPlan", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CompanyName")
                        .HasColumnName("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Passengers")
                        .HasColumnName("Passengers")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("FlightPlans");
                });

            modelBuilder.Entity("FlightControlWeb.Model.InitialLocation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("FlightPlanId")
                        .HasColumnType("bigint");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("FlightPlanId")
                        .IsUnique();

                    b.ToTable("InitialLocation");
                });

            modelBuilder.Entity("FlightControlWeb.Model.Location", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("FlightPlanId")
                        .HasColumnType("bigint");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<int>("TimeSpanSeconds")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FlightPlanId");

                    b.ToTable("Segments");
                });

            modelBuilder.Entity("FlightControlWeb.Model.Server", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ServerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServerURL")
                        .HasColumnName("ServerURL")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("FlightControlWeb.Model.Flight", b =>
                {
                    b.HasOne("FlightControlWeb.Model.FlightPlan", "FlightPlan")
                        .WithOne("Flight")
                        .HasForeignKey("FlightControlWeb.Model.Flight", "FlightPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FlightControlWeb.Model.InitialLocation", b =>
                {
                    b.HasOne("FlightControlWeb.Model.FlightPlan", null)
                        .WithOne("InitialLocation")
                        .HasForeignKey("FlightControlWeb.Model.InitialLocation", "FlightPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FlightControlWeb.Model.Location", b =>
                {
                    b.HasOne("FlightControlWeb.Model.FlightPlan", "FlightPlan")
                        .WithMany("Segments")
                        .HasForeignKey("FlightPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
