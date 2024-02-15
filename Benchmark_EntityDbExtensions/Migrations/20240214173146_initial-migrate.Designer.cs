﻿// <auto-generated />
using System;
using Benchmark_EntityDbExtensions.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Benchmark_EntityDbExtensions.Migrations
{
    [DbContext(typeof(SqlLiteDbContext))]
    [Migration("20240214173146_initial-migrate")]
    partial class initialmigrate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("Benchmark_EntityDbExtensions.Entities.ChildClass", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RootClassId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RootClassId");

                    b.ToTable("ChildClass");
                });

            modelBuilder.Entity("Benchmark_EntityDbExtensions.Entities.GrandChildClass", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ChildClassId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChildClassId");

                    b.ToTable("GrandChildClass");
                });

            modelBuilder.Entity("Benchmark_EntityDbExtensions.Entities.RootClass", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("RootClass");
                });

            modelBuilder.Entity("Benchmark_EntityDbExtensions.Entities.ChildClass", b =>
                {
                    b.HasOne("Benchmark_EntityDbExtensions.Entities.RootClass", "RootClass")
                        .WithMany("ChildClass")
                        .HasForeignKey("RootClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RootClass");
                });

            modelBuilder.Entity("Benchmark_EntityDbExtensions.Entities.GrandChildClass", b =>
                {
                    b.HasOne("Benchmark_EntityDbExtensions.Entities.ChildClass", "ChildClass")
                        .WithMany("GrandChildClass")
                        .HasForeignKey("ChildClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChildClass");
                });

            modelBuilder.Entity("Benchmark_EntityDbExtensions.Entities.ChildClass", b =>
                {
                    b.Navigation("GrandChildClass");
                });

            modelBuilder.Entity("Benchmark_EntityDbExtensions.Entities.RootClass", b =>
                {
                    b.Navigation("ChildClass");
                });
#pragma warning restore 612, 618
        }
    }
}
