using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class LmsContext : DbContext
    {

///Test
    public LmsContext(DbContextOptions<LmsContext>
    options):base(options)
    {
    }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DeliveryType> DeliveryTypes { get; set; }
        public virtual DbSet<Division> Divisions { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<InboundDocument> InboundDocuments { get; set; }
        public virtual DbSet<InboundItem> InboundItems { get; set; }
        public virtual DbSet<InboundTripCharge> InboundTripCharges { get; set; }
        public virtual DbSet<InboundTripImage> InboundTripImages { get; set; }
        public virtual DbSet<InboundTripMaster> InboundTripMasters { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<MovementType> MovementTypes { get; set; }
        public virtual DbSet<OutboundDocument> OutboundDocuments { get; set; }
        public virtual DbSet<OutboundItem> OutboundItems { get; set; }
        public virtual DbSet<OutboundTrip> OutboundTrips { get; set; }
        public virtual DbSet<OutboundTripCharge> OutboundTripCharges { get; set; }
        public virtual DbSet<OutboundTripImage> OutboundTripImages { get; set; }
        public virtual DbSet<OutboundTripMaster> OutboundTripMasters { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductUnit> ProductUnits { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<SecurityGate> SecurityGates { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<TransportCompany> TransportCompanies { get; set; }
        public virtual DbSet<TripPayer> TripPayers { get; set; }
        public virtual DbSet<TripPurpose> TripPurposes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VehicleMaster> VehicleMasters { get; set; }
        public virtual DbSet<VehicleRegistration> VehicleRegistrations { get; set; }
        public virtual DbSet<VehicleType> VehicleTypes { get; set; }

    
            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerName).IsUnicode(false);
            });

            modelBuilder.Entity<DeliveryType>(entity =>
            {
                entity.Property(e => e.DeliveryType1).IsUnicode(false);
            });

            modelBuilder.Entity<Division>(entity =>
            {
                entity.Property(e => e.Abbr).IsUnicode(false);

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.DayLag).HasDefaultValueSql("((10))");

                entity.Property(e => e.DivisionName).IsUnicode(false);

                entity.Property(e => e.Dmsdbname).IsUnicode(false);

                entity.Property(e => e.Header).IsUnicode(false);
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.Property(e => e.DriverName).IsUnicode(false);
            });

            modelBuilder.Entity<InboundDocument>(entity =>
            {
                entity.HasIndex(e => e.SupplierId);

                entity.HasIndex(e => new { e.DivisionId, e.DocumentId, e.DocumentNo, e.LocationId, e.MovementTypeId, e.Remarks, e.SupplierId, e.InboundTripId })
                    .HasName("IDX_InboundDocuments_InboundTripId");

                entity.Property(e => e.DocumentNo).IsUnicode(false);

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.HasOne(d => d.Division)
                    .WithMany(p => p.InboundDocuments)
                    .HasForeignKey(d => d.DivisionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inbound_Divisions");

                entity.HasOne(d => d.InboundTrip)
                    .WithMany(p => p.InboundDocuments)
                    .HasForeignKey(d => d.InboundTripId)
                    .HasConstraintName("FK_Inbound_InboundTripMaster");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.InboundDocuments)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inbound_Locations");

                entity.HasOne(d => d.MovementType)
                    .WithMany(p => p.InboundDocuments)
                    .HasForeignKey(d => d.MovementTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InboundDocuments_MovementTypes");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.InboundDocuments)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inbound_Suppliers1");
            });

            modelBuilder.Entity<InboundItem>(entity =>
            {
                entity.HasIndex(e => new { e.ProductId, e.Qty, e.DocumentId })
                    .HasName("IX_InboundItems");

                entity.HasIndex(e => new { e.Id, e.ProductId, e.Qty, e.DocumentId })
                    .HasName("IDX_InboundItems_DocumentId");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.InboundItems)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK_InboundItems_InboundDocuments");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.InboundItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemDetails_Products");
            });

            modelBuilder.Entity<InboundTripCharge>(entity =>
            {
                entity.Property(e => e.InboundTripId).ValueGeneratedNever();

                entity.Property(e => e.TripRemarks).IsUnicode(false);

                entity.HasOne(d => d.InboundTrip)
                    .WithOne(p => p.InboundTripCharge)
                    .HasForeignKey<InboundTripCharge>(d => d.InboundTripId)
                    .HasConstraintName("FK_InboundTripCharges_InboundTripMaster");
            });

            modelBuilder.Entity<InboundTripImage>(entity =>
            {
                entity.Property(e => e.ImageName).IsUnicode(false);

                entity.Property(e => e.LocalPath).IsUnicode(false);

                entity.Property(e => e.ServerPath).IsUnicode(false);

                entity.HasOne(d => d.InboundTrip)
                    .WithMany(p => p.InboundTripImages)
                    .HasForeignKey(d => d.InboundTripId)
                    .HasConstraintName("FK_InboundTripImages_InboundTripMaster");
            });

            modelBuilder.Entity<InboundTripMaster>(entity =>
            {
                entity.HasIndex(e => new { e.DriverId, e.Id, e.VehicleId, e.ChargedDivisionId, e.TripDate })
                    .HasName("IX_InboundTripMaster_ChargedDivisionId_TripDate");

                entity.HasIndex(e => new { e.DriverId, e.Id, e.SecurityGateId, e.TripCharge, e.VehicleId, e.TripDate })
                    .HasName("IDX_InboundTripMaster_TripDate");

                entity.Property(e => e.CameraRemarks).IsUnicode(false);

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.TripPayerId).HasDefaultValueSql("((1))");

                entity.Property(e => e.TripRemarks).IsUnicode(false);

                entity.HasOne(d => d.ChargedDivision)
                    .WithMany(p => p.InboundTripMasters)
                    .HasForeignKey(d => d.ChargedDivisionId)
                    .HasConstraintName("FK_InboundTripMaster_Divisions");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.InboundTripMasters)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InboundTripMaster_Drivers");

                entity.HasOne(d => d.Purpose)
                    .WithMany(p => p.InboundTripMasters)
                    .HasForeignKey(d => d.PurposeId)
                    .HasConstraintName("FK_InboundTripMaster_TripPurpose");

                entity.HasOne(d => d.SecurityGate)
                    .WithMany(p => p.InboundTripMasters)
                    .HasForeignKey(d => d.SecurityGateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InboundTripMaster_SecurityGates");

                entity.HasOne(d => d.Source)
                    .WithMany(p => p.InboundTripMasters)
                    .HasForeignKey(d => d.SourceId)
                    .HasConstraintName("FK_InboundTripMaster_Locations");

                entity.HasOne(d => d.TripPayer)
                    .WithMany(p => p.InboundTripMasters)
                    .HasForeignKey(d => d.TripPayerId)
                    .HasConstraintName("FK_InboundTripMaster_TripPayers");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.InboundTripMasters)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InboundTripMaster_VehicleMaster");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.LocationName).IsUnicode(false);
            });

            modelBuilder.Entity<MovementType>(entity =>
            {
                entity.Property(e => e.Direction)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('I')");

                entity.Property(e => e.MovementType1).IsUnicode(false);
            });

            modelBuilder.Entity<OutboundDocument>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.OutboundDocuments)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutboundDocuments_Customers");

                entity.HasOne(d => d.DeliveryType)
                    .WithMany(p => p.OutboundDocuments)
                    .HasForeignKey(d => d.DeliveryTypeId)
                    .HasConstraintName("FK_OutboundDocuments_DeliveryTypes");

                entity.HasOne(d => d.Division)
                    .WithMany(p => p.OutboundDocuments)
                    .HasForeignKey(d => d.DivisionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutboundDocuments_Divisions");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.OutboundDocuments)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutboundDocuments_Locations");

                entity.HasOne(d => d.MovementType)
                    .WithMany(p => p.OutboundDocuments)
                    .HasForeignKey(d => d.MovementTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutboundDocuments_MovementTypes");

                entity.HasOne(d => d.OutboundTrip)
                    .WithMany(p => p.OutboundDocuments)
                    .HasForeignKey(d => d.OutboundTripId)
                    .HasConstraintName("FK_OutboundDocuments_OutboundTripMaster");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.OutboundDocuments)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutboundDocuments_Projects");
            });

            modelBuilder.Entity<OutboundItem>(entity =>
            {
                entity.HasOne(d => d.Document)
                    .WithMany(p => p.OutboundItems)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK_OutboundItems_OutboundDocuments");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OutboundItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Outbound_Products");
            });

            modelBuilder.Entity<OutboundTrip>(entity =>
            {
                entity.HasOne(d => d.OutboundTripNavigation)
                    .WithMany(p => p.OutboundTrips)
                    .HasForeignKey(d => d.OutboundTripId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutboundTrips_OutboundTripMaster");
            });

            modelBuilder.Entity<OutboundTripCharge>(entity =>
            {
                entity.Property(e => e.OutboundTripId).ValueGeneratedNever();

                entity.Property(e => e.TripRemarks).IsUnicode(false);

                entity.HasOne(d => d.OutboundTrip)
                    .WithOne(p => p.OutboundTripCharge)
                    .HasForeignKey<OutboundTripCharge>(d => d.OutboundTripId)
                    .HasConstraintName("FK_OutboundTripCharges_OutboundTripMaster");
            });

            modelBuilder.Entity<OutboundTripImage>(entity =>
            {
                entity.Property(e => e.ImageName).IsUnicode(false);

                entity.Property(e => e.LocalPath).IsUnicode(false);

                entity.Property(e => e.ServerPath).IsUnicode(false);

                entity.HasOne(d => d.OutboundTrip)
                    .WithMany(p => p.OutboundTripImages)
                    .HasForeignKey(d => d.OutboundTripId)
                    .HasConstraintName("FK_OutboundTripImages_OutboundTripMaster");
            });

            modelBuilder.Entity<OutboundTripMaster>(entity =>
            {
                entity.HasIndex(e => new { e.SecurityGateId, e.TripDate })
                    .HasName("OutBoundMaster_Date_Gate");

                entity.HasIndex(e => new { e.ChargedDivisionId, e.TripCharge, e.VehicleId, e.TripDate })
                    .HasName("IX_OutboundTripMaster_TripDate");

                entity.HasIndex(e => new { e.DriverId, e.Id, e.VehicleId, e.ChargedDivisionId, e.TripDate })
                    .HasName("IX_OutboundTripMaster_ChargedDivisionId_TripDate");

                entity.Property(e => e.CameraRemarks).IsUnicode(false);

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.TripPayerId).HasDefaultValueSql("((1))");

                entity.Property(e => e.TripRemarks).IsUnicode(false);

                entity.HasOne(d => d.ChargedDivision)
                    .WithMany(p => p.OutboundTripMasters)
                    .HasForeignKey(d => d.ChargedDivisionId)
                    .HasConstraintName("FK_OutboundTripMaster_Divisions");

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.OutboundTripMasters)
                    .HasForeignKey(d => d.DestinationId)
                    .HasConstraintName("FK_OutboundTripMaster_Locations");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.OutboundTripMasters)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutboundTripMaster_Drivers");

                entity.HasOne(d => d.Purpose)
                    .WithMany(p => p.OutboundTripMasters)
                    .HasForeignKey(d => d.PurposeId)
                    .HasConstraintName("FK_OutboundTripMaster_TripPurpose");

                entity.HasOne(d => d.SecurityGate)
                    .WithMany(p => p.OutboundTripMasters)
                    .HasForeignKey(d => d.SecurityGateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutboundTripMaster_SecurityGates");

                entity.HasOne(d => d.TripPayer)
                    .WithMany(p => p.OutboundTripMasters)
                    .HasForeignKey(d => d.TripPayerId)
                    .HasConstraintName("FK_OutboundTripMaster_TripPayers");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OutboundTripMasters)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_OutboundTripMaster_Users");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.OutboundTripMasters)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutboundTripMaster_VehicleMaster");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductName).IsUnicode(false);

                entity.Property(e => e.ProductNameAbs).IsUnicode(false);

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_Products_ProductUnits");
            });

            modelBuilder.Entity<ProductUnit>(entity =>
            {
                entity.Property(e => e.Unit).IsUnicode(false);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.ProjectName).IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Projects_Customers");
            });

            modelBuilder.Entity<SecurityGate>(entity =>
            {
                entity.Property(e => e.SecurityGateName).IsUnicode(false);
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(e => e.SupplierName).IsUnicode(false);
            });

            modelBuilder.Entity<TransportCompany>(entity =>
            {
                entity.Property(e => e.TransportCompanyName).IsUnicode(false);
            });

            modelBuilder.Entity<TripPayer>(entity =>
            {
                entity.Property(e => e.TripPayer1).IsUnicode(false);
            });

            modelBuilder.Entity<TripPurpose>(entity =>
            {
                entity.Property(e => e.Purpose).IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.UserName).IsUnicode(false);
            });

            modelBuilder.Entity<VehicleMaster>(entity =>
            {
                entity.Property(e => e.TruckNo).IsUnicode(false);

                entity.Property(e => e.VehicleTypeId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Registration)
                    .WithMany(p => p.VehicleMasters)
                    .HasForeignKey(d => d.RegistrationId)
                    .HasConstraintName("FK_VehicleMaster_VehicleRegistrations");

                entity.HasOne(d => d.TransportCompany)
                    .WithMany(p => p.VehicleMasters)
                    .HasForeignKey(d => d.TransportCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VehicleMaster_TransportCompanies");

                entity.HasOne(d => d.VehicleType)
                    .WithMany(p => p.VehicleMasters)
                    .HasForeignKey(d => d.VehicleTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VehicleMaster_VehicleTypes");
            });

            modelBuilder.Entity<VehicleRegistration>(entity =>
            {
                entity.HasKey(e => e.RegistrationId)
                    .HasName("PK_VehicleRegistrations_1");

                entity.Property(e => e.Registration).IsUnicode(false);
            });

            modelBuilder.Entity<VehicleType>(entity =>
            {
                entity.Property(e => e.VehicleType1).IsUnicode(false);
            });

            OnModelCreatingExt(modelBuilder);
        }

        partial void OnModelCreatingExt(ModelBuilder modelBuilder);

    }
    }
