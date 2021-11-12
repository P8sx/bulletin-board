using BulletinBoard.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace BulletinBoard.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, ulong, IdentityUserClaim<ulong>, UserRole, IdentityUserLogin<ulong>, IdentityRoleClaim<ulong>, IdentityUserToken<ulong>>
    {
        public virtual DbSet<Group>? Groups { get; set; }
        public virtual DbSet<Bulletin>? Bulletins { get; set; }
        public virtual DbSet<Comment>? Comments { get; set; }



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<UserRole>().ToTable("UserRoles");
            builder.Entity<IdentityUserLogin<ulong>>().ToTable("UserLogins");
            builder.Entity<IdentityUserClaim<ulong>>().ToTable("UserClaims");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<IdentityRoleClaim<ulong>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<ulong>>().ToTable("UserTokens");

            builder.Entity<Bulletin>().Property(p => p.AttachmentFiles)
            .HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                v => JsonSerializer.Deserialize<List<Guid>>(v, new JsonSerializerOptions()));
            var valueComparer = new ValueComparer<List<Guid>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());
            builder
                .Entity<Bulletin>()
                .Property(e => e.AttachmentFiles)
                .Metadata
                .SetValueComparer(valueComparer);

            builder.Entity<User>()
                .HasMany(left => left.Groups)
                .WithMany(right => right.Users)
                .UsingEntity(join => join.ToTable("GroupUsers"));
        }
    }
}