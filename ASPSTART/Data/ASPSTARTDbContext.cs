using Microsoft.EntityFrameworkCore;
using ASPSTART.Data.Entities;

namespace ASPSTART.Data;
public class ASPSTARTDbContext : DbContext
{
    public ASPSTARTDbContext(DbContextOptions<ASPSTARTDbContext> opt) : base(opt) { }

    public DbSet<CateEntity> Categories { get; set; }
    public DbSet<UserEntity> Users { get; set; }
}
