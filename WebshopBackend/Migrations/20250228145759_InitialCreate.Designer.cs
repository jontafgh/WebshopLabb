﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebshopBackend;

#nullable disable

namespace WebshopBackend.Migrations
{
    [DbContext(typeof(WebshopContext))]
    [Migration("20250228145759_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebshopBackend.Models.Boardgame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArtNr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("BoardgameDetailsId")
                        .HasColumnType("int");

                    b.Property<int?>("ImageId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PriceId")
                        .HasColumnType("int");

                    b.Property<int?>("PublisherId")
                        .HasColumnType("int");

                    b.Property<int?>("StockId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BoardgameDetailsId");

                    b.HasIndex("ImageId");

                    b.HasIndex("PriceId");

                    b.HasIndex("PublisherId");

                    b.HasIndex("StockId");

                    b.ToTable("Boardgames");
                });

            modelBuilder.Entity("WebshopBackend.Models.BoardgameDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxPlayers")
                        .HasColumnType("int");

                    b.Property<int>("MinAge")
                        .HasColumnType("int");

                    b.Property<int>("MinPlayers")
                        .HasColumnType("int");

                    b.Property<int>("PlayTime")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("BoardgameDetails");
                });

            modelBuilder.Entity("WebshopBackend.Models.Discount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("WebshopBackend.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageLargeUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageMediumUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageSmallUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("WebshopBackend.Models.Price", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("DiscountId")
                        .HasColumnType("int");

                    b.Property<decimal>("Regular")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("DiscountId");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("WebshopBackend.Models.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("WebshopBackend.Models.Restock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("RestockDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Restock");
                });

            modelBuilder.Entity("WebshopBackend.Models.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("NextRestockId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NextRestockId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("WebshopBackend.Models.Boardgame", b =>
                {
                    b.HasOne("WebshopBackend.Models.BoardgameDetails", "BoardgameDetails")
                        .WithMany()
                        .HasForeignKey("BoardgameDetailsId");

                    b.HasOne("WebshopBackend.Models.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.HasOne("WebshopBackend.Models.Price", "Price")
                        .WithMany()
                        .HasForeignKey("PriceId");

                    b.HasOne("WebshopBackend.Models.Publisher", "Publisher")
                        .WithMany()
                        .HasForeignKey("PublisherId");

                    b.HasOne("WebshopBackend.Models.Stock", "Stock")
                        .WithMany()
                        .HasForeignKey("StockId");

                    b.Navigation("BoardgameDetails");

                    b.Navigation("Image");

                    b.Navigation("Price");

                    b.Navigation("Publisher");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("WebshopBackend.Models.Price", b =>
                {
                    b.HasOne("WebshopBackend.Models.Discount", "Discount")
                        .WithMany()
                        .HasForeignKey("DiscountId");

                    b.Navigation("Discount");
                });

            modelBuilder.Entity("WebshopBackend.Models.Stock", b =>
                {
                    b.HasOne("WebshopBackend.Models.Restock", "NextRestock")
                        .WithMany()
                        .HasForeignKey("NextRestockId");

                    b.Navigation("NextRestock");
                });
#pragma warning restore 612, 618
        }
    }
}
