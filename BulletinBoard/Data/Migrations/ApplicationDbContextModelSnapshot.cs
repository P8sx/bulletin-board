﻿// <auto-generated />
using System;
using BulletinBoard.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BulletinBoard.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BulletinBoard.Model.Bulletin", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("AttachmentFiles")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<uint?>("DownVotes")
                        .HasColumnType("int unsigned");

                    b.Property<DateTime?>("Expired")
                        .HasColumnType("datetime(6)");

                    b.Property<ulong?>("GroupId")
                        .HasColumnType("bigint unsigned");

                    b.Property<float?>("Latitude")
                        .HasColumnType("float");

                    b.Property<float?>("Longitude")
                        .HasColumnType("float");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime(6)");

                    b.Property<bool?>("Pinned")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<uint?>("UpVotes")
                        .HasColumnType("int unsigned");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("Bulletins");
                });

            modelBuilder.Entity("BulletinBoard.Model.Comment", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong?>("BulletinId")
                        .HasColumnType("bigint unsigned");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Text")
                        .HasColumnType("longtext");

                    b.Property<ulong?>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("BulletinId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("BulletinBoard.Model.Group", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<bool?>("Public")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("BulletinBoard.Model.Role", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<int>("RoleValue")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("BulletinBoard.Model.User", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<Guid?>("Avatar")
                        .HasColumnType("char(36)");

                    b.Property<string>("City")
                        .HasColumnType("longtext");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Country")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PostCode")
                        .HasColumnType("longtext");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<string>("Street")
                        .HasColumnType("longtext");

                    b.Property<string>("StreetNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("BulletinBoard.Model.UserRole", b =>
                {
                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("RoleId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong?>("GroupId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("GroupId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.Property<ulong>("GroupsId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("UsersId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("GroupsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("GroupUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<ulong>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<ulong>("RoleId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<ulong>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<ulong>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<ulong>", b =>
                {
                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("BulletinBoard.Model.Bulletin", b =>
                {
                    b.HasOne("BulletinBoard.Model.Group", "Group")
                        .WithMany("Bulletins")
                        .HasForeignKey("GroupId");

                    b.HasOne("BulletinBoard.Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BulletinBoard.Model.Comment", b =>
                {
                    b.HasOne("BulletinBoard.Model.Bulletin", "Bulletin")
                        .WithMany("Comments")
                        .HasForeignKey("BulletinId");

                    b.HasOne("BulletinBoard.Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Bulletin");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BulletinBoard.Model.UserRole", b =>
                {
                    b.HasOne("BulletinBoard.Model.Group", "Group")
                        .WithMany("UserRoles")
                        .HasForeignKey("GroupId");

                    b.HasOne("BulletinBoard.Model.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BulletinBoard.Model.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.HasOne("BulletinBoard.Model.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BulletinBoard.Model.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<ulong>", b =>
                {
                    b.HasOne("BulletinBoard.Model.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<ulong>", b =>
                {
                    b.HasOne("BulletinBoard.Model.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<ulong>", b =>
                {
                    b.HasOne("BulletinBoard.Model.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<ulong>", b =>
                {
                    b.HasOne("BulletinBoard.Model.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BulletinBoard.Model.Bulletin", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("BulletinBoard.Model.Group", b =>
                {
                    b.Navigation("Bulletins");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
