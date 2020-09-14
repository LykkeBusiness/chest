﻿// <auto-generated />
using System;
using Chest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Chest.Data.Migrations.ChestDb
{
    [DbContext(typeof(ChestDbContext))]
    partial class ChestDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("chest")
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Chest.Data.Entities.AuditEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CorrelationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataDiff")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AuditTrail");
                });

            modelBuilder.Entity("Chest.Data.Entities.Locale", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Locales");
                });

            modelBuilder.Entity("Chest.Data.Entities.LocalizedValue", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Locale")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(4096);

                    b.HasKey("Key", "Locale");

                    b.ToTable("LocalizedValues");
                });
#pragma warning restore 612, 618
        }
    }
}
