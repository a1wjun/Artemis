﻿// <auto-generated />
using System;
using Artemis.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Artemis.Storage.Migrations
{
    [DbContext(typeof(ArtemisDbContext))]
    [Migration("20250204194848_DevicesClearBrokenJson")]
    partial class DevicesClearBrokenJson
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("Artemis.Storage.Entities.General.ReleaseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("InstalledAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("InstalledAt");

                    b.HasIndex("Version")
                        .IsUnique();

                    b.ToTable("Releases");
                });

            modelBuilder.Entity("Artemis.Storage.Entities.Plugins.PluginEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("PluginGuid")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PluginGuid")
                        .IsUnique();

                    b.ToTable("Plugins");
                });

            modelBuilder.Entity("Artemis.Storage.Entities.Plugins.PluginFeatureEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("PluginEntityId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PluginEntityId");

                    b.ToTable("PluginFeatures");
                });

            modelBuilder.Entity("Artemis.Storage.Entities.Plugins.PluginSettingEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PluginGuid")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PluginGuid");

                    b.HasIndex("Name", "PluginGuid")
                        .IsUnique();

                    b.ToTable("PluginSettings");
                });

            modelBuilder.Entity("Artemis.Storage.Entities.Profile.ProfileCategoryEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCollapsed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSuspended")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ProfileCategories");
                });

            modelBuilder.Entity("Artemis.Storage.Entities.Profile.ProfileContainerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Icon")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("Profile")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProfileCategoryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProfileConfiguration")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProfileCategoryId");

                    b.ToTable("ProfileContainers");
                });

            modelBuilder.Entity("Artemis.Storage.Entities.Surface.DeviceEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<float>("BlueScale")
                        .HasColumnType("REAL");

                    b.PrimitiveCollection<string>("Categories")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceProvider")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<float>("GreenScale")
                        .HasColumnType("REAL");

                    b.Property<string>("InputIdentifiers")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("InputMappings")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LayoutParameter")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("LayoutType")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("LogicalLayout")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<int>("PhysicalLayout")
                        .HasColumnType("INTEGER");

                    b.Property<float>("RedScale")
                        .HasColumnType("REAL");

                    b.Property<float>("Rotation")
                        .HasColumnType("REAL");

                    b.Property<float>("Scale")
                        .HasColumnType("REAL");

                    b.Property<float>("X")
                        .HasColumnType("REAL");

                    b.Property<float>("Y")
                        .HasColumnType("REAL");

                    b.Property<int>("ZIndex")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Artemis.Storage.Entities.Workshop.EntryEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("AutoUpdate")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Categories")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<long>("Downloads")
                        .HasColumnType("INTEGER");

                    b.Property<long>("EntryId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EntryType")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("InstalledAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsOfficial")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LatestReleaseId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Metadata")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("ReleaseId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReleaseVersion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EntryId")
                        .IsUnique();

                    b.ToTable("Entries");
                });

            modelBuilder.Entity("Artemis.Storage.Entities.Plugins.PluginFeatureEntity", b =>
                {
                    b.HasOne("Artemis.Storage.Entities.Plugins.PluginEntity", null)
                        .WithMany("Features")
                        .HasForeignKey("PluginEntityId");
                });

            modelBuilder.Entity("Artemis.Storage.Entities.Profile.ProfileContainerEntity", b =>
                {
                    b.HasOne("Artemis.Storage.Entities.Profile.ProfileCategoryEntity", "ProfileCategory")
                        .WithMany("ProfileConfigurations")
                        .HasForeignKey("ProfileCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProfileCategory");
                });

            modelBuilder.Entity("Artemis.Storage.Entities.Plugins.PluginEntity", b =>
                {
                    b.Navigation("Features");
                });

            modelBuilder.Entity("Artemis.Storage.Entities.Profile.ProfileCategoryEntity", b =>
                {
                    b.Navigation("ProfileConfigurations");
                });
#pragma warning restore 612, 618
        }
    }
}
