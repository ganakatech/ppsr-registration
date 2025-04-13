﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PpsrRegistration.Data;

#nullable disable

namespace ppsr_registration.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.4");

            modelBuilder.Entity("PpsrRegistration.Models.VehicleRegistration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Duration")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GrantorFirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GrantorLastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GrantorMiddleNames")
                        .HasColumnType("TEXT");

                    b.Property<string>("SPG_ACN")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SPG_OrgName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("VIN")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Registrations");
                });
#pragma warning restore 612, 618
        }
    }
}
