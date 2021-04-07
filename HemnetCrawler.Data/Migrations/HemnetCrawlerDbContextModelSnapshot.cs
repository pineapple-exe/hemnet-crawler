﻿// <auto-generated />
using System;
using HemnetCrawler.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HemnetCrawler.Data.Migrations
{
    [DbContext(typeof(HemnetCrawlerDbContext))]
    partial class HemnetCrawlerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HemnetCrawler.Data.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("ListingID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ListingID");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("HemnetCrawler.Data.Entities.Listing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Balcony")
                        .HasColumnType("bit");

                    b.Property<double?>("BiArea")
                        .HasColumnType("float");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(96)
                        .HasColumnType("nvarchar(96)");

                    b.Property<int?>("ConstructionYear")
                        .HasColumnType("int");

                    b.Property<int>("DaysOnHemnet")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnergyClassification")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int?>("Fee")
                        .HasColumnType("int");

                    b.Property<string>("Floor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeOwnersAssociation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeType")
                        .IsRequired()
                        .HasMaxLength(22)
                        .HasColumnType("nvarchar(22)");

                    b.Property<double?>("LivingArea")
                        .HasColumnType("float");

                    b.Property<bool>("NewConstruction")
                        .HasColumnType("bit");

                    b.Property<string>("OwnershipType")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<int?>("Price")
                        .HasColumnType("int");

                    b.Property<int?>("PricePerSquareMeter")
                        .HasColumnType("int");

                    b.Property<int>("PropertyArea")
                        .HasColumnType("int");

                    b.Property<int?>("Rooms")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(96)
                        .HasColumnType("nvarchar(96)");

                    b.Property<int?>("Utilities")
                        .HasColumnType("int");

                    b.Property<int>("Visits")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Listings");
                });

            modelBuilder.Entity("HemnetCrawler.Data.Entities.Image", b =>
                {
                    b.HasOne("HemnetCrawler.Data.Entities.Listing", "Listing")
                        .WithMany()
                        .HasForeignKey("ListingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Listing");
                });
#pragma warning restore 612, 618
        }
    }
}
