using Microsoft.EntityFrameworkCore;

namespace DotNetCardsServer.Services.Data
{
    public class ApplicationDbContext :DbContext
    {
        public DbSet<UserSqlModel> Users { get; set; }
        public DbSet<CardSqlModel> Cards { get; set; }
        public DbSet<UserCardLike> CardLikes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserCardLike>()
                .HasOne(ucl => ucl.User)
                .WithMany()
                .HasForeignKey(ucl => ucl.User_Id)
                .OnDelete(DeleteBehavior.Cascade);

           modelBuilder.Entity<UserCardLike>()
                .HasOne(ucl => ucl.Card)
                .WithMany()
                .HasForeignKey(ucl => ucl.Card_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
