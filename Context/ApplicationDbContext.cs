using CP.Api.Models;

using Microsoft.EntityFrameworkCore;

namespace CP.Api.Context;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<Vote> Votes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Vote>().HasKey(v => new { v.AccountId, v.CommentId });
        modelBuilder.Entity<Role>().HasData(DefaultRoles.Administrator, DefaultRoles.User, DefaultRoles.Manager);
    }
}
