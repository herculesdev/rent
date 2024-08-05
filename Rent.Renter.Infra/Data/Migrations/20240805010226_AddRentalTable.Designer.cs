﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Rent.Renter.Infra.Data.Contexts;

#nullable disable

namespace Rent.Renter.Infra.Data.Migrations
{
    [DbContext(typeof(RenterContext))]
    [Migration("20240805010226_AddRentalTable")]
    partial class AddRentalTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Rent.Renter.Core.Entities.DeliveryPerson", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date")
                        .HasColumnName("BirthDate");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedAtUtc");

                    b.Property<DateTime?>("DeletedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("DeletedAtUtc");

                    b.Property<string>("DocumentNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("DocumentNumber");

                    b.Property<string>("DriverLicenseImageName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("DriverLicenseImageName");

                    b.Property<string>("DriverLicenseNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("DriverLicenseNumber");

                    b.Property<string>("DriverLicenseType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("DriverLicenseType");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Name");

                    b.Property<DateTime>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedAtUtc");

                    b.HasKey("Id");

                    b.HasIndex("DocumentNumber")
                        .IsUnique();

                    b.HasIndex("DriverLicenseNumber")
                        .IsUnique();

                    b.ToTable("DeliveryPerson", (string)null);
                });

            modelBuilder.Entity("Rent.Renter.Core.Entities.Rental", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedAtUtc");

                    b.Property<DateTime?>("DeletedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("DeletedAtUtc");

                    b.Property<Guid>("DeliveryPersonId")
                        .HasColumnType("uuid")
                        .HasColumnName("DeliveryPersonId");

                    b.Property<DateOnly?>("EffectiveEndDateUtc")
                        .HasColumnType("date")
                        .HasColumnName("EffectiveEndDateUtc");

                    b.Property<DateOnly>("EstimatedEndDateUtc")
                        .HasColumnType("date")
                        .HasColumnName("EstimatedEndDateUtc");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("IsDeleted");

                    b.Property<Guid>("MotorbikeId")
                        .HasColumnType("uuid")
                        .HasColumnName("MotorbikeId");

                    b.Property<Guid>("PlanId")
                        .HasColumnType("uuid")
                        .HasColumnName("PlanId");

                    b.Property<DateOnly>("StartDateUtc")
                        .HasColumnType("date")
                        .HasColumnName("StartDateUtc");

                    b.Property<DateTime>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedAtUtc");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryPersonId");

                    b.ToTable("Rental", (string)null);
                });

            modelBuilder.Entity("Rent.Renter.Core.Entities.Rental", b =>
                {
                    b.HasOne("Rent.Renter.Core.Entities.DeliveryPerson", "DeliveryPerson")
                        .WithMany()
                        .HasForeignKey("DeliveryPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeliveryPerson");
                });
#pragma warning restore 612, 618
        }
    }
}