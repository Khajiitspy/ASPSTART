using Microsoft.EntityFrameworkCore;
using ASPSTART.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ASPSTART.Data.Entities.Identity;

namespace ASPSTART.Data;
public class ASPSTARTDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
{
    public ASPSTARTDbContext(DbContextOptions<ASPSTARTDbContext> opt) : base(opt) { }

    public DbSet<CateEntity> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // identity 
        modelBuilder.Entity<UserRoleEntity>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRoleEntity>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

    }
}
