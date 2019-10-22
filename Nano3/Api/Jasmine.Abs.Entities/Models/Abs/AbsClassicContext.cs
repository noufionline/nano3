using Microsoft.EntityFrameworkCore;

namespace Jasmine.Abs.Entities.Models.Abs
{
    public class AbsClassicContext : DbContext
    {


        public AbsClassicContext(DbContextOptions<AbsClassicContext> options) : base(options)
        {

        }


        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Customer>()
                .HasKey(x => x.CustomerId);

            modelBuilder.Entity<Customer>().Property(p => p.CustomerId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Customer>()
                .Property(e => e.CustomerName)
                .IsUnicode(false);

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Projects");
                entity.HasKey(e => e.ProjectId);
                entity.Property(e => e.ProjectId).ValueGeneratedOnAdd();

                entity.HasIndex(e => e.CustomerId);

                entity.Property(e => e.ProjectName).IsUnicode(false);

                entity.HasOne(d => d.Customer)
                   .WithMany(p => p.Projects)
                   .HasForeignKey(d => d.CustomerId)
                   .HasConstraintName("FK_Projects_Customers");
            });

            modelBuilder.Entity<SalesOrder>().ToTable("SalesOrders");
            modelBuilder.Entity<SalesOrder>().HasKey(x => x.SalesOrderId);
            modelBuilder.Entity<SalesOrder>().Property(x => x.SalesOrderId).ValueGeneratedOnAdd();

            modelBuilder.Entity<SalesOrder>().HasOne(t => t.Customer)
                .WithMany(x => x.SalesOrders)
                .HasForeignKey(x => x.CustomerId);


            modelBuilder.Entity<Document>().ToTable("SalesOrderDocuments");
            modelBuilder.Entity<Document>().HasKey(x => x.DocumentId);
            modelBuilder.Entity<Document>().Property(x => x.DocumentId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Document>().HasOne(t => t.Order)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.SalesOrderId);
        }


        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //    modelBuilder.Entity<Customer>().ToTable("Customers");
        //    modelBuilder.Entity<Customer>()
        //        .HasKey(x => x.CustomerId);

        //    modelBuilder.Entity<Customer>().Property(p => p.CustomerId)
        //        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        //    modelBuilder.Entity<Customer>()
        //        .Property(e => e.CustomerName)
        //        .IsUnicode(false);


        //    modelBuilder.Entity<SalesOrder>().ToTable("SalesOrders");
        //    modelBuilder.Entity<SalesOrder>().HasKey(x => x.SalesOrderId);
        //    modelBuilder.Entity<SalesOrder>().Property(x => x.SalesOrderId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        //    modelBuilder.Entity<SalesOrder>().HasRequired(t => t.Customer)
        //        .WithMany(x => x.SalesOrders)
        //        .HasForeignKey(x => x.CustomerId);


        //    modelBuilder.Entity<Document>().ToTable("SalesOrderDocuments");
        //    modelBuilder.Entity<Document>().HasKey(x => x.DocumentId);
        //    modelBuilder.Entity<Document>().Property(x => x.DocumentId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        //    modelBuilder.Entity<Document>().HasRequired(t => t.Order)
        //        .WithMany(x => x.Documents)
        //        .HasForeignKey(x => x.SalesOrderId);



        //}
    }


}

