using Microsoft.EntityFrameworkCore;

namespace Jasmine.AbsCore.Entities.Models.Zeon
{
    public partial class ZeonContext : DbContext
    {


        public ZeonContext(DbContextOptions<ZeonContext> options) : base(options)
        {

        }

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

        public virtual DbSet<AbsDivision> AbsDivisions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AbsDivision>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ApplicationName)
                    .HasMaxLength(256);

                entity.Property(e => e.ApplicationType)
                    .HasMaxLength(256);

                entity.Property(e => e.InitialCatelog)
                    .HasMaxLength(256);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.EmployeeName).IsRequired();

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });
        }
    }
}
