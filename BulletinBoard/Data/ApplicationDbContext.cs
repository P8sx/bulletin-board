using BulletinBoard.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace BulletinBoard.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, ulong, IdentityUserClaim<ulong>, IdentityUserRole<ulong>, IdentityUserLogin<ulong>, IdentityRoleClaim<ulong>, IdentityUserToken<ulong>>
    {
        public virtual DbSet<Group> Groups { get; set; } = default!;
        public virtual DbSet<GroupUser> GroupUsers { get; set; } = default!;
        public virtual DbSet<Bulletin> Bulletins { get; set; } = default!;
        public virtual DbSet<Comment> Comments { get; set; } = default!;
        public virtual DbSet<BulletinVote> BulletinsVotes { get; set;} = default!;
        public virtual DbSet<BulletinBookmark> BulletinBookmarks { get; set; } = default!;
        public virtual DbSet<Violation> Violations { get; set; } = default!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityUserRole<ulong>>().ToTable("UserRoles");
            builder.Entity<IdentityUserLogin<ulong>>().ToTable("UserLogins");
            builder.Entity<IdentityUserClaim<ulong>>().ToTable("UserClaims");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<IdentityRoleClaim<ulong>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<ulong>>().ToTable("UserTokens");

            builder.Entity<BulletinVote>()
                .HasKey(sc => new { sc.UserId, sc.BulletinId });

            builder.Entity<BulletinBookmark>()
                .HasKey(sc => new { sc.UserId, sc.BulletinId });
            //builder.Entity<User>()
            //    .HasMany(left => left.Groups)
            //    .WithMany(right => right.Users)
            //    .UsingEntity(join => join.ToTable("GroupUsers"));

        }
    }
}