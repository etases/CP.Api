using CP.Api.Models;

using Microsoft.EntityFrameworkCore;

namespace CP.Api.Context
{
    /// <summary>
    /// Db Context of the application
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="options"> Db Context options</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Account table
        /// </summary>
        public virtual DbSet<Account> Accounts { get; set; } = null!;

        /// <summary>
        /// Role table
        /// </summary>
        public virtual DbSet<Role> Roles { get; set; } = null!;

        /// <summary>
        /// Category table
        /// </summary>
        public virtual DbSet<Category> Categories { get; set; } = null!;

        /// <summary>
        /// Comment table
        /// </summary>
        public virtual DbSet<Comment> Comments { get; set; } = null!;

        /// <summary>
        /// Vote table
        /// </summary>
        public virtual DbSet<Vote> Votes { get; set; } = null!;

        /// <summary>
        /// Configure entity models to be used in the application
        /// </summary>
        /// <param name="modelBuilder">Entity model constructor</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            _ = modelBuilder.Entity<Vote>().HasKey(v => new { v.AccountId, v.CommentId });
            _ = modelBuilder.Entity<Role>().HasData(DefaultRoles.Administrator, DefaultRoles.User, DefaultRoles.Manager);
        }
    }
}
