﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YBS2.Data.Context;

#nullable disable

namespace YBS2.Data.Migrations
{
    [DbContext(typeof(YBS2Context))]
    [Migration("20231230072315_Add Transaction")]
    partial class AddTransaction
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("YBS2.Data.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("YBS2.Data.Models.Booking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("MemberId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TotalPassengers")
                        .HasColumnType("int");

                    b.Property<Guid>("TourId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("TourId");

                    b.ToTable("Booking", (string)null);
                });

            modelBuilder.Entity("YBS2.Data.Models.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("FacebookURL")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("HotLine")
                        .IsRequired()
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("InstagramURL")
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("LinkedInURL")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("LogoURL")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("Company", (string)null);
                });

            modelBuilder.Entity("YBS2.Data.Models.Dock", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Dock", (string)null);
                });

            modelBuilder.Entity("YBS2.Data.Models.Member", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("AvatarURL")
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("date");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("IdentityNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime?>("MemberSinceDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(12)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("Member", (string)null);
                });

            modelBuilder.Entity("YBS2.Data.Models.MembershipPackage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.Property<float>("DiscountPercent")
                        .HasColumnType("real");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("DurationUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("MembershipPackage", (string)null);
                });

            modelBuilder.Entity("YBS2.Data.Models.MembershipRegistration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("MembershipExpireDate")
                        .HasColumnType("date");

                    b.Property<Guid>("MembershipPackageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("MembershipStartDate")
                        .HasColumnType("date");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("MembershipPackageId");

                    b.ToTable("MembershipRegistration", (string)null);
                });

            modelBuilder.Entity("YBS2.Data.Models.Passenger", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("date");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("IdentityNumber")
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.ToTable("Passenger", (string)null);
                });

            modelBuilder.Entity("YBS2.Data.Models.Tour", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("DurationUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("MaximumGuest")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<Guid?>("YachtId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("YachtId");

                    b.ToTable("Tour", (string)null);
                });

            modelBuilder.Entity("YBS2.Data.Models.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BankCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("BankTranNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid?>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CardType")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid?>("MembershipRegistrationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Success")
                        .HasColumnType("bit");

                    b.Property<float>("TotalAmount")
                        .HasColumnType("real");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("VNPayCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("MembershipRegistrationId")
                        .IsUnique()
                        .HasFilter("[MembershipRegistrationId] IS NOT NULL");

                    b.ToTable("Transaction", (string)null);
                });

            modelBuilder.Entity("YBS2.Data.Models.UpdateRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("ApprovedDate")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("ApproverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("FacebookURL")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("HotLine")
                        .IsRequired()
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("InstagramURL")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("LinkedInURL")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("LogoURL")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApproverId");

                    b.HasIndex("CompanyId");

                    b.ToTable("UpdateRequest", (string)null);
                });

            modelBuilder.Entity("YBS2.Data.Models.Yacht", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BEAM")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Cabin")
                        .HasColumnType("int");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DRAFT")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("LOA")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TotalCrew")
                        .HasColumnType("int");

                    b.Property<int>("TotalPassenger")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Yacht", (string)null);
                });

            modelBuilder.Entity("YBS2.Data.Models.Booking", b =>
                {
                    b.HasOne("YBS2.Data.Models.Member", "Member")
                        .WithMany("Bookings")
                        .HasForeignKey("MemberId");

                    b.HasOne("YBS2.Data.Models.Tour", "Tour")
                        .WithMany("Bookings")
                        .HasForeignKey("TourId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("Tour");
                });

            modelBuilder.Entity("YBS2.Data.Models.Company", b =>
                {
                    b.HasOne("YBS2.Data.Models.Account", "Account")
                        .WithOne("Company")
                        .HasForeignKey("YBS2.Data.Models.Company", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("YBS2.Data.Models.Dock", b =>
                {
                    b.HasOne("YBS2.Data.Models.Company", "Company")
                        .WithMany("Docks")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("YBS2.Data.Models.Member", b =>
                {
                    b.HasOne("YBS2.Data.Models.Account", "Account")
                        .WithOne("Member")
                        .HasForeignKey("YBS2.Data.Models.Member", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("YBS2.Data.Models.MembershipRegistration", b =>
                {
                    b.HasOne("YBS2.Data.Models.Member", "Member")
                        .WithMany("MembershipRegistrations")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YBS2.Data.Models.MembershipPackage", "MembershipPackage")
                        .WithMany("MembershipRegistrations")
                        .HasForeignKey("MembershipPackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("MembershipPackage");
                });

            modelBuilder.Entity("YBS2.Data.Models.Passenger", b =>
                {
                    b.HasOne("YBS2.Data.Models.Booking", "Booking")
                        .WithMany("Passengers")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("YBS2.Data.Models.Tour", b =>
                {
                    b.HasOne("YBS2.Data.Models.Company", "Company")
                        .WithMany("Tours")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YBS2.Data.Models.Yacht", "Yacht")
                        .WithMany("Tours")
                        .HasForeignKey("YachtId");

                    b.Navigation("Company");

                    b.Navigation("Yacht");
                });

            modelBuilder.Entity("YBS2.Data.Models.Transaction", b =>
                {
                    b.HasOne("YBS2.Data.Models.Booking", "Booking")
                        .WithMany("Transactions")
                        .HasForeignKey("BookingId");

                    b.HasOne("YBS2.Data.Models.MembershipRegistration", "MembershipRegistration")
                        .WithOne("Transaction")
                        .HasForeignKey("YBS2.Data.Models.Transaction", "MembershipRegistrationId");

                    b.Navigation("Booking");

                    b.Navigation("MembershipRegistration");
                });

            modelBuilder.Entity("YBS2.Data.Models.UpdateRequest", b =>
                {
                    b.HasOne("YBS2.Data.Models.Account", "Account")
                        .WithMany("UpdateRequests")
                        .HasForeignKey("ApproverId");

                    b.HasOne("YBS2.Data.Models.Company", "Company")
                        .WithMany("UpdateRequests")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("YBS2.Data.Models.Yacht", b =>
                {
                    b.HasOne("YBS2.Data.Models.Company", "Company")
                        .WithMany("Yachts")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("YBS2.Data.Models.Account", b =>
                {
                    b.Navigation("Company");

                    b.Navigation("Member");

                    b.Navigation("UpdateRequests");
                });

            modelBuilder.Entity("YBS2.Data.Models.Booking", b =>
                {
                    b.Navigation("Passengers");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("YBS2.Data.Models.Company", b =>
                {
                    b.Navigation("Docks");

                    b.Navigation("Tours");

                    b.Navigation("UpdateRequests");

                    b.Navigation("Yachts");
                });

            modelBuilder.Entity("YBS2.Data.Models.Member", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("MembershipRegistrations");
                });

            modelBuilder.Entity("YBS2.Data.Models.MembershipPackage", b =>
                {
                    b.Navigation("MembershipRegistrations");
                });

            modelBuilder.Entity("YBS2.Data.Models.MembershipRegistration", b =>
                {
                    b.Navigation("Transaction")
                        .IsRequired();
                });

            modelBuilder.Entity("YBS2.Data.Models.Tour", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("YBS2.Data.Models.Yacht", b =>
                {
                    b.Navigation("Tours");
                });
#pragma warning restore 612, 618
        }
    }
}