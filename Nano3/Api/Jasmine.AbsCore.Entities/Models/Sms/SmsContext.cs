using Microsoft.EntityFrameworkCore;

namespace Jasmine.AbsCore.Entities.Models.Sms
{
    public partial class SmsContext : DbContext
    {

        public SmsContext(DbContextOptions<SmsContext> options):base(options)
        {
            
        }
        public virtual DbSet<ReportRepository> ReportRepository { get; set; }
        public virtual DbSet<SteelOpeningStocks> SteelOpeningStocks { get; set; }
        public virtual DbSet<SteelOrigins> SteelOrigins { get; set; }
        public virtual DbSet<SteelProducts> SteelProducts { get; set; }
        public virtual DbSet<SteelTypes> SteelTypes { get; set; }
        public virtual DbSet<StockItems> StockItems { get; set; }
        public virtual DbSet<StockLocations> StockLocations { get; set; }
        public virtual DbSet<StockSubLocations> StockSubLocations { get; set; }
        public virtual DbSet<UserSessions> UserSessions { get; set; }

     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportRepository>(entity =>
            {
                entity.HasKey(e => e.RepositoryId)
                    .HasName("PK_ReportRepository");

                entity.Property(e => e.BarCountWeight).HasColumnType("decimal");

                entity.Property(e => e.Diameter)
                    .IsRequired()
                    .HasColumnType("varchar(7)");

                entity.Property(e => e.IsSpecialLength).HasDefaultValueSql("0");

                entity.Property(e => e.Length)
                    .HasColumnType("decimal")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NoOfRolls).HasDefaultValueSql("0");

                entity.Property(e => e.Origin).HasColumnType("varchar(50)");

                entity.Property(e => e.OriginName)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnType("nchar(100)");

                entity.Property(e => e.TheoreticalWeight).HasColumnType("decimal");

                entity.Property(e => e.TransactionType).HasColumnType("varchar(50)");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.ReportRepository)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ReportRepository_StockLocations");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.ReportRepository)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("FK_ReportRepository_UserSessions");

                entity.HasOne(d => d.SteelType)
                    .WithMany(p => p.ReportRepository)
                    .HasForeignKey(d => d.SteelTypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ReportRepository_SteelTypes");

                entity.HasOne(d => d.SubLocation)
                    .WithMany(p => p.ReportRepository)
                    .HasForeignKey(d => d.SubLocationId)
                    .HasConstraintName("FK_ReportRepository_StockSubLocations");
            });

            modelBuilder.Entity<SteelOpeningStocks>(entity =>
            {
                entity.HasKey(e => e.StockId)
                    .HasName("PK_SteelOpeningStocks_1");

                entity.Property(e => e.BarCountWeight).HasColumnType("decimal");

                entity.Property(e => e.ItemCode).HasColumnType("varchar(25)");

                entity.Property(e => e.NoOfRolls).HasDefaultValueSql("0");

                entity.Property(e => e.TheoreticalWeight).HasColumnType("decimal");

                entity.Property(e => e.TransactionDate).HasColumnType("smalldatetime");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.SteelOpeningStocks)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SteelOpeningStocks_StockLocations");

                entity.HasOne(d => d.StockItem)
                    .WithMany(p => p.SteelOpeningStocks)
                    .HasForeignKey(d => d.StockItemId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SteelOpeningStocks_StockItems");

                entity.HasOne(d => d.SubLocation)
                    .WithMany(p => p.SteelOpeningStocks)
                    .HasForeignKey(d => d.SubLocationId)
                    .HasConstraintName("FK_SteelOpeningStocks_StockSubLocation");
            });

            modelBuilder.Entity<SteelOrigins>(entity =>
            {
                entity.HasKey(e => e.OriginId)
                    .HasName("PK_SteelOrigins");

                entity.Property(e => e.OriginId).ValueGeneratedNever();

                entity.Property(e => e.OriginCode)
                    .IsRequired()
                    .HasColumnType("varchar(3)");

                entity.Property(e => e.OriginName)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<SteelProducts>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK_SteelProducts");

                entity.Property(e => e.ProductId).ValueGeneratedNever();

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.SteelType)
                    .WithMany(p => p.SteelProducts)
                    .HasForeignKey(d => d.SteelTypeId)
                    .HasConstraintName("FK_SteelProducts_SteelTypes");
            });

            modelBuilder.Entity<SteelTypes>(entity =>
            {
                entity.HasKey(e => e.SteelTypeId)
                    .HasName("PK_SteelTypes");

                entity.Property(e => e.SteelTypeId).ValueGeneratedNever();

                entity.Property(e => e.SteelType)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<StockItems>(entity =>
            {
                entity.HasKey(e => e.StockItemId)
                    .HasName("PK_StockItems");

                entity.HasIndex(e => e.ItemCode)
                    .HasName("IX_StockItems_ItemCode")
                    .IsUnique();

                entity.Property(e => e.Diameter)
                    .IsRequired()
                    .HasColumnType("varchar(25)");

                entity.Property(e => e.ItemCode)
                    .IsRequired()
                    .HasColumnType("varchar(25)");

                entity.Property(e => e.Length)
                    .HasColumnType("decimal")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Origin)
                    .WithMany(p => p.StockItems)
                    .HasForeignKey(d => d.OriginId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_StockItems_SteelOrigins");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.StockItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_StockItems_SteelProducts");
            });

            modelBuilder.Entity<StockLocations>(entity =>
            {
                entity.HasKey(e => e.LocationId)
                    .HasName("PK_StockLocations");

                entity.Property(e => e.LocationId).ValueGeneratedNever();

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnType("varchar(150)");
            });

            modelBuilder.Entity<StockSubLocations>(entity =>
            {
                entity.HasKey(e => e.SubLocationId)
                    .HasName("PK_StockSubLocation");

                entity.Property(e => e.SubLocation)
                    .IsRequired()
                    .HasColumnType("varchar(150)");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.StockSubLocations)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_StockSubLocation_StockLocations");
            });

            modelBuilder.Entity<UserSessions>(entity =>
            {
                entity.HasKey(e => e.SessionId)
                    .HasName("PK_UserSessions");

                entity.Property(e => e.Date).HasColumnType("smalldatetime");

                entity.Property(e => e.RegDate)
                    .HasColumnName("Reg_Date")
                    .HasColumnType("datetime");
            });
        }
    }
}