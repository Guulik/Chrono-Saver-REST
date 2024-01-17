using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccessLayer
{
    public class ChronoDbContext: DbContext
    {
        public ChronoDbContext(DbContextOptions<ChronoDbContext> options): base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasKey(x => x.Id);
            builder.Entity<User>().HasMany(x=>x.Games).WithOne(x => x.User).HasForeignKey("UserID");

            builder.Entity<Game>().HasKey(x => x.Id);
            builder.Entity<Game>().HasMany(x => x.Saves).WithOne(x => x.Game).HasForeignKey("GameID");
            builder.Entity<Game>().HasOne(x => x.User).WithMany(x => x.Games).HasForeignKey("UserID");

            builder.Entity<Save>().HasKey(x => x.Id);
            builder.Entity<Save>().HasOne(x => x.Game).WithMany(x => x.Saves).HasForeignKey("GameID");
        }
        
    }
}
