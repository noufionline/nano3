using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class AbsContext : DbContext
    {
        public virtual DbSet<AbsDatabas> AbsDatabases { get; set; }
        public virtual DbSet<AccountReceivable> AccountReceivables { get; set; }
        public virtual DbSet<AccountReceivableCollector> AccountReceivableCollectors { get; set; }
        public virtual DbSet<AgingFromSunSystem> AgingFromSunSystems { get; set; }
        public virtual DbSet<AllocatedInvoice> AllocatedInvoices { get; set; }
        public virtual DbSet<ApplicationSetting> ApplicationSettings { get; set; }
        public virtual DbSet<ApprovedOriginsForQuotation> ApprovedOriginsForQuotations { get; set; }
        public virtual DbSet<Approver> Approvers { get; set; }
        public virtual DbSet<AttachmentType> AttachmentTypes { get; set; }
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public virtual DbSet<AuditLogLine> AuditLogLines { get; set; }
        public virtual DbSet<AutoMailInfo> AutoMailInfoes { get; set; }
        public virtual DbSet<AutoNotificationInfo> AutoNotificationInfoes { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<BankDepositSlip> BankDepositSlips { get; set; }
        public virtual DbSet<BankDocumentAttachment> BankDocumentAttachments { get; set; }
        public virtual DbSet<BankDocumentAttachmentType> BankDocumentAttachmentTypes { get; set; }
        public virtual DbSet<BankDocumentTransactionHistory> BankDocumentTransactionHistories { get; set; }
        public virtual DbSet<BusinessType> BusinessTypes { get; set; }
        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
        public virtual DbSet<CommercialInvoice> CommercialInvoices { get; set; }
        public virtual DbSet<CommercialInvoiceAttachment> CommercialInvoiceAttachments { get; set; }
        public virtual DbSet<CommercialInvoiceAttachmentType> CommercialInvoiceAttachmentTypes { get; set; }
        public virtual DbSet<CommercialInvoiceTransactionHistory> CommercialInvoiceTransactionHistories { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<CustomersFromSunSystem> CustomersFromSunSystems { get; set; }
        public virtual DbSet<CustomersFromSunSystemBackup> CustomersFromSunSystemBackups { get; set; }
        public virtual DbSet<DebtorStatement> DebtorStatements { get; set; }
        public virtual DbSet<DebtorStatementChequesInHandLine> DebtorStatementChequesInHandLines { get; set; }
        public virtual DbSet<DebtorStatementInvoiceLine> DebtorStatementInvoiceLines { get; set; }
        public virtual DbSet<DeliveryPoint> DeliveryPoints { get; set; }
        public virtual DbSet<Division> Divisions { get; set; }
        public virtual DbSet<EmiratesOrCountry> EmiratesOrCountries { get; set; }
        public virtual DbSet<Follower> Followers { get; set; }
        public virtual DbSet<IssuancePlace> IssuancePlaces { get; set; }
        public virtual DbSet<JournalVoucher> JournalVouchers { get; set; }
        public virtual DbSet<JournalVoucherLine> JournalVoucherLines { get; set; }
        public virtual DbSet<JournalVoucherReceipt> JournalVoucherReceipts { get; set; }
        public virtual DbSet<JournalVoucherReceiptLine> JournalVoucherReceiptLines { get; set; }
        public virtual DbSet<LcDocument> LcDocuments { get; set; }
        public virtual DbSet<LcDocumentRevision> LcDocumentRevisions { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<OdooSunAccount> OdooSunAccounts { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<OrderType> OrderTypes { get; set; }
        public virtual DbSet<OwnerIdentityDocument> OwnerIdentityDocuments { get; set; }
        public virtual DbSet<Partner> Partners { get; set; }
        public virtual DbSet<PartnerAttachment> PartnerAttachments { get; set; }
        public virtual DbSet<PartnerBanker> PartnerBankers { get; set; }
        public virtual DbSet<PartnerContact> PartnerContacts { get; set; }
        public virtual DbSet<PartnerGroup> PartnerGroups { get; set; }
        public virtual DbSet<PartnerGroupMapping> PartnerGroupMappings { get; set; }
        public virtual DbSet<PartnerGroupType> PartnerGroupTypes { get; set; }
        public virtual DbSet<PartnerLegalFormType> PartnerLegalFormTypes { get; set; }
        public virtual DbSet<PartnerOwner> PartnerOwners { get; set; }
        public virtual DbSet<PartnerPaymentTerm> PartnerPaymentTerms { get; set; }
        public virtual DbSet<PartnerPaymentTermsTest> PartnerPaymentTermsTests { get; set; }
        public virtual DbSet<PartnerProduct> PartnerProducts { get; set; }
        public virtual DbSet<PartnerProject> PartnerProjects { get; set; }
        public virtual DbSet<PartnerRating> PartnerRatings { get; set; }
        public virtual DbSet<PartnerTradeReference> PartnerTradeReferences { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<PaymentMethodByTerm> PaymentMethodByTerms { get; set; }
        public virtual DbSet<PaymentReceiptVoucher> PaymentReceiptVouchers { get; set; }
        public virtual DbSet<PaymentReceiver> PaymentReceivers { get; set; }
        public virtual DbSet<PaymentTerm> PaymentTerms { get; set; }
        public virtual DbSet<PaymentTermsAll> PaymentTermsAlls { get; set; }
        public virtual DbSet<PaymentTermsWithPartnerId> PaymentTermsWithPartnerIds { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public virtual DbSet<ProductUnit> ProductUnits { get; set; }
        public virtual DbSet<Quotation> Quotations { get; set; }
        public virtual DbSet<QuotationAttachment> QuotationAttachments { get; set; }
        public virtual DbSet<QuotationAttachmentType> QuotationAttachmentTypes { get; set; }
        public virtual DbSet<QuotationContact> QuotationContacts { get; set; }
        public virtual DbSet<QuotationEmail> QuotationEmails { get; set; }
        public virtual DbSet<QuotationLine> QuotationLines { get; set; }
        public virtual DbSet<QuotationMiscProduct> QuotationMiscProducts { get; set; }
        public virtual DbSet<QuotationSalesCondition> QuotationSalesConditions { get; set; }
        public virtual DbSet<QuotationStateLog> QuotationStateLogs { get; set; }
        public virtual DbSet<Reminder> Reminders { get; set; }
        public virtual DbSet<SalesCondition> SalesConditions { get; set; }
        public virtual DbSet<SalesOrder> SalesOrders { get; set; }
        public virtual DbSet<SalesOrderAttachment> SalesOrderAttachments { get; set; }
        public virtual DbSet<SalesOrderAttachmentType> SalesOrderAttachmentTypes { get; set; }
        public virtual DbSet<SalesOrderLine> SalesOrderLines { get; set; }
        public virtual DbSet<SalesOrderQuotation> SalesOrderQuotations { get; set; }
        public virtual DbSet<SalesPerson> SalesPersons { get; set; }
        public virtual DbSet<SalesPersonsByDivision> SalesPersonsByDivisions { get; set; }
        public virtual DbSet<SalesTermsByProductCategory> SalesTermsByProductCategories { get; set; }
        public virtual DbSet<SalesTermsMaster> SalesTermsMasters { get; set; }
        public virtual DbSet<SunAccountCodeGroupMapping> SunAccountCodeGroupMappings { get; set; }
        public virtual DbSet<SunAccountGroup> SunAccountGroups { get; set; }
        public virtual DbSet<SunSystemAllocatedInvoice> SunSystemAllocatedInvoices { get; set; }
        public virtual DbSet<SunSystemDataFile> SunSystemDataFiles { get; set; }
        public virtual DbSet<SunSystemUnAllocatedInvoice> SunSystemUnAllocatedInvoices { get; set; }
        public virtual DbSet<TaskAttachment> TaskAttachments { get; set; }
        public virtual DbSet<TaskAttachmentType> TaskAttachmentTypes { get; set; }
        public virtual DbSet<TechnicalContact> TechnicalContacts { get; set; }
        public virtual DbSet<TestPaymentMethodGroupsByCustomer> TestPaymentMethodGroupsByCustomers { get; set; }
        public virtual DbSet<TradeReferenceType> TradeReferenceTypes { get; set; }
        public virtual DbSet<UnitsByProductCategory> UnitsByProductCategories { get; set; }
        public virtual DbSet<UserTask> UserTasks { get; set; }
        public virtual DbSet<VQuotationStatu> VQuotationStatus { get; set; }

        public AbsContext(DbContextOptions<AbsContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=192.168.30.31; Initial Catalog=AbsCoreDevelopment; User Id=sa;pwd=fkt");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AbsDatabas>(entity =>
            {
                entity.Property(e => e.DivisionName).IsUnicode(false);

                entity.Property(e => e.InitialCatalog).IsUnicode(false);
            });

            modelBuilder.Entity<AccountReceivable>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasName("IX_AccountReceivables_ReceiptNo")
                    .IsUnique();

                entity.Property(e => e.CollectionVoucherId).HasDefaultValueSql("(NEXT VALUE FOR [CollectioVoucherSequence])");

                entity.Property(e => e.CollectionVoucherNo).IsUnicode(false);

                entity.Property(e => e.CompanyId).HasDefaultValueSql("((2))");

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.DocumentNo).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.PaymentStatusId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.SunAccountCode).IsUnicode(false);

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.AccountReceivables)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK_AccountReceivables_Banks");

                entity.HasOne(d => d.CollectedBy)
                    .WithMany(p => p.AccountReceivables)
                    .HasForeignKey(d => d.CollectedById)
                    .HasConstraintName("FK_AccountReceivables_AccountReceivableCollectors");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.AccountReceivables)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountReceivables_Companies");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.AccountReceivables)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountReceivables_Partners");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.AccountReceivables)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_AccountReceivables_PartnerProjects");
            });

            modelBuilder.Entity<AccountReceivableCollector>(entity =>
            {
                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<AgingFromSunSystem>(entity =>
            {
                entity.Property(e => e.AccountCode).IsUnicode(false);
            });

            modelBuilder.Entity<AllocatedInvoice>(entity =>
            {
                entity.Property(e => e.InvoiceNo).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.BankDocument)
                    .WithMany(p => p.AllocatedInvoices)
                    .HasForeignKey(d => d.BankDocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AllocatedInvoices_AccountReceivables");
            });

            modelBuilder.Entity<ApplicationSetting>(entity =>
            {
                entity.Property(e => e.Property).IsUnicode(false);

                entity.Property(e => e.PropertyType).IsUnicode(false);

                entity.Property(e => e.Value).IsUnicode(false);
            });

            modelBuilder.Entity<ApprovedOriginsForQuotation>(entity =>
            {
                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<Approver>(entity =>
            {
                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.Designation).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<AttachmentType>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_AttachmentTypes")
                    .IsUnique();

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.Property(e => e.After).IsUnicode(false);

                entity.Property(e => e.Before).IsUnicode(false);

                entity.Property(e => e.Changes).IsUnicode(false);

                entity.Property(e => e.EntityName).IsUnicode(false);

                entity.Property(e => e.RegUser).IsUnicode(false);
            });

            modelBuilder.Entity<AuditLogLine>(entity =>
            {
                entity.Property(e => e.After).IsUnicode(false);

                entity.Property(e => e.Before).IsUnicode(false);

                entity.Property(e => e.EntityName).IsUnicode(false);

                entity.Property(e => e.PropertyName).IsUnicode(false);

                entity.Property(e => e.PropertyType).IsUnicode(false);

                entity.HasOne(d => d.AuditLog)
                    .WithMany(p => p.AuditLogLines)
                    .HasForeignKey(d => d.AuditLogId)
                    .HasConstraintName("FK_AuditLogLines_AuditLogs");
            });

            modelBuilder.Entity<AutoMailInfo>(entity =>
            {
                entity.Property(e => e.Body).IsUnicode(false);

                entity.Property(e => e.FromAddress).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.Subject).IsUnicode(false);

                entity.Property(e => e.ToAddress).IsUnicode(false);
            });

            modelBuilder.Entity<AutoNotificationInfo>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_AutoNotificationInfo")
                    .IsUnique();

                entity.Property(e => e.AutoDismiss).HasDefaultValueSql("((1))");

                entity.Property(e => e.CollectionViewName).IsUnicode(false);

                entity.Property(e => e.CommandText).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Filter).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Parameters).IsUnicode(false);

                entity.Property(e => e.Type).IsUnicode(false);
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_Banks")
                    .IsUnique();

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<BankDepositSlip>(entity =>
            {
                entity.Property(e => e.Depositor).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.SlipNo).IsUnicode(false);

                entity.Property(e => e.Telephone).IsUnicode(false);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.BankDepositSlips)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_BankDepositSlips_Companies");
            });

            modelBuilder.Entity<BankDocumentAttachment>(entity =>
            {
                entity.Property(e => e.AttachmentVersion).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.AttachmentType)
                    .WithMany(p => p.BankDocumentAttachments)
                    .HasForeignKey(d => d.AttachmentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BankDocumentAttachments_BankDocumentAttachmentTypes");

                entity.HasOne(d => d.BankDocument)
                    .WithMany(p => p.BankDocumentAttachments)
                    .HasForeignKey(d => d.BankDocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BankDocumentAttachments_AccountReceivables");
            });

            modelBuilder.Entity<BankDocumentAttachmentType>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedUser)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<BankDocumentTransactionHistory>(entity =>
            {
                entity.Property(e => e.Reason).IsUnicode(false);

                entity.HasOne(d => d.BankDocument)
                    .WithMany(p => p.BankDocumentTransactionHistories)
                    .HasForeignKey(d => d.BankDocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BankDocumentTransactionHistory_AccountReceivables");

                entity.HasOne(d => d.DepositSlip)
                    .WithMany(p => p.BankDocumentTransactionHistories)
                    .HasForeignKey(d => d.DepositSlipId)
                    .HasConstraintName("FK_BankDocumentTransactionHistory_BankDepositSlips");
            });

            modelBuilder.Entity<BusinessType>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_BusinessTypes")
                    .IsUnique();

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.Property(e => e.Author).IsUnicode(false);

                entity.Property(e => e.Receiver).IsUnicode(false);
            });

            modelBuilder.Entity<CommercialInvoice>(entity =>
            {
                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.DraftCopyVersion).IsUnicode(false);

                entity.Property(e => e.InvoiceNo).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.SignedCopyVersion).IsUnicode(false);

                entity.Property(e => e.StatusRemarks).IsUnicode(false);

                entity.HasOne(d => d.LcDocument)
                    .WithMany(p => p.CommercialInvoices)
                    .HasForeignKey(d => d.LcDocumentId)
                    .HasConstraintName("FK_LcDocumentLines_LcDocuments");
            });

            modelBuilder.Entity<CommercialInvoiceAttachment>(entity =>
            {
                entity.Property(e => e.AttachmentVersion).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.AttachmentType)
                    .WithMany(p => p.CommercialInvoiceAttachments)
                    .HasForeignKey(d => d.AttachmentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommercialInvoiceAttachments_CommercialInvoiceAttachmentTypes");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.CommercialInvoiceAttachments)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK_CommercialInvoiceAttachments_CommercialInvoices");
            });

            modelBuilder.Entity<CommercialInvoiceAttachmentType>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedUser)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<CommercialInvoiceTransactionHistory>(entity =>
            {
                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.CommercialInvoiceTransactionHistories)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK_CommercialInvoiceTransactionHistory_CommercialInvoices");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.AccountNo).IsUnicode(false);

                entity.Property(e => e.BankName).IsUnicode(false);

                entity.Property(e => e.Branch).IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedUser)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<CustomersFromSunSystem>(entity =>
            {
                entity.Property(e => e.CustomerCode).IsUnicode(false);

                entity.Property(e => e.AccountName).IsUnicode(false);

                entity.Property(e => e.ActualAccount).IsUnicode(false);

                entity.Property(e => e.AddressCode).IsUnicode(false);

                entity.Property(e => e.PaymentTermGroupCode).IsUnicode(false);

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.CustomersFromSunSystems)
                    .HasForeignKey(d => d.PartnerId)
                    .HasConstraintName("FK_CustomersFromSunSystem_Partners");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.CustomersFromSunSystems)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_CustomersFromSunSystem_PartnerProjects");
            });

            modelBuilder.Entity<CustomersFromSunSystemBackup>(entity =>
            {
                entity.Property(e => e.CustomerCode).IsUnicode(false);

                entity.Property(e => e.ActualAccount).IsUnicode(false);

                entity.Property(e => e.AddressCode).IsUnicode(false);
            });

            modelBuilder.Entity<DebtorStatement>(entity =>
            {
                entity.Property(e => e.AccountingPeriod).IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedUser)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.SunAccounts).IsUnicode(false);

                entity.HasOne(d => d.PartnerGroup)
                    .WithMany(p => p.DebtorStatements)
                    .HasForeignKey(d => d.PartnerGroupId)
                    .HasConstraintName("FK_DebtorStatements_PartnerGroups");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.DebtorStatements)
                    .HasForeignKey(d => d.PartnerId)
                    .HasConstraintName("FK_DebtorStatements_Partners");
            });

            modelBuilder.Entity<DebtorStatementChequesInHandLine>(entity =>
            {
                entity.Property(e => e.Bank).IsUnicode(false);

                entity.Property(e => e.DocumentNo).IsUnicode(false);

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.SunAccountCode).IsUnicode(false);

                entity.HasOne(d => d.DebtorStatement)
                    .WithMany(p => p.DebtorStatementChequesInHandLines)
                    .HasForeignKey(d => d.DebtorStatementId)
                    .HasConstraintName("FK_DebtorStatementChequesInHandLines_DebtorStatements");
            });

            modelBuilder.Entity<DebtorStatementInvoiceLine>(entity =>
            {
                entity.Property(e => e.AccountCode).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.InvoiceNo).IsUnicode(false);

                entity.Property(e => e.InvoiceStatus).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.DebtorStatement)
                    .WithMany(p => p.DebtorStatementInvoiceLines)
                    .HasForeignKey(d => d.DebtorStatementId)
                    .HasConstraintName("FK_DebtorStatmentInvoiceLines_DebtorStatements");
            });

            modelBuilder.Entity<DeliveryPoint>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_DeliveryPoints")
                    .IsUnique();

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedUser)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('System')");

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<Division>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Abbr).IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedUser)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('System')");

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<EmiratesOrCountry>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_EmiratesOrCountries")
                    .IsUnique();

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<Follower>(entity =>
            {
                entity.Property(e => e.FollowingUser).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.Followers)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Followers_Tasks");
            });

            modelBuilder.Entity<IssuancePlace>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_IssuancePlaces")
                    .IsUnique();

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<JournalVoucher>(entity =>
            {
                entity.Property(e => e.Branch).IsUnicode(false);

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.JournalRef).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Purpose).IsUnicode(false);

                entity.Property(e => e.Reason).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.VoucherNo).IsUnicode(false);

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.JournalVouchers)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JournalVouchers_Partners");
            });

            modelBuilder.Entity<JournalVoucherLine>(entity =>
            {
                entity.Property(e => e.AccountNo).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.TransactionType).IsUnicode(false);

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.JournalVoucherLines)
                    .HasForeignKey(d => d.VoucherId)
                    .HasConstraintName("FK_JournalVoucherLines_JournalVouchers");
            });

            modelBuilder.Entity<JournalVoucherReceipt>(entity =>
            {
                entity.Property(e => e.Branch).IsUnicode(false);

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.JournalRef).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Purpose).IsUnicode(false);

                entity.Property(e => e.Reason).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.VoucherNo).IsUnicode(false);

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.JournalVoucherReceipts)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JournalVoucherReceipts_Partners");
            });

            modelBuilder.Entity<JournalVoucherReceiptLine>(entity =>
            {
                entity.Property(e => e.AccountNo).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.TransactionType).IsUnicode(false);

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.JournalVoucherReceiptLines)
                    .HasForeignKey(d => d.VoucherId)
                    .HasConstraintName("FK_JournalVoucherRecieptLines_JournalVoucherReceipts");
            });

            modelBuilder.Entity<LcDocument>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasName("IX_LcDocuments_LcNo")
                    .IsUnique();

                entity.Property(e => e.ClientBankName).IsUnicode(false);

                entity.Property(e => e.ClientLcNo).IsUnicode(false);

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.SunAccountCode).IsUnicode(false);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.LcDocuments)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LcDocuments_Companies");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.LcDocuments)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LcDocuments_Partners");

                entity.HasOne(d => d.SunAccountCodeNavigation)
                    .WithMany(p => p.LcDocuments)
                    .HasForeignKey(d => d.SunAccountCode)
                    .HasConstraintName("FK_LcDocuments_CustomersFromSunSystem");
            });

            modelBuilder.Entity<LcDocumentRevision>(entity =>
            {
                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.LcDocumentRevisions)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK_LcDocumentRevisions_LcDocuments");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.EntityName).IsUnicode(false);

                entity.Property(e => e.LogNote).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<Nationality>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_Nationalities")
                    .IsUnique();

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<OdooSunAccount>(entity =>
            {
                entity.Property(e => e.PartnerName).IsUnicode(false);

                entity.Property(e => e.ProjectName).IsUnicode(false);

                entity.Property(e => e.SunAccountCode).IsUnicode(false);

                entity.Property(e => e.SunDb).IsUnicode(false);
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<OrderType>(entity =>
            {
                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<OwnerIdentityDocument>(entity =>
            {
                entity.HasIndex(e => e.NationalityId);

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.EidfollowUpHistory).IsUnicode(false);

                entity.Property(e => e.EidfollowUpNote).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.EmiratesIdAttachmentPath).IsUnicode(false);

                entity.Property(e => e.EmiratesIdNo).IsUnicode(false);

                entity.Property(e => e.Fax).IsUnicode(false);

                entity.Property(e => e.FollowUpHistory).IsUnicode(false);

                entity.Property(e => e.FollowUpNote).IsUnicode(false);

                entity.Property(e => e.Mobile).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.NameOnPassport).IsUnicode(false);

                entity.Property(e => e.PassportAttachmentPath).IsUnicode(false);

                entity.Property(e => e.PassportIssuedPlace).IsUnicode(false);

                entity.Property(e => e.PassportNo).IsUnicode(false);

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.Telephone).IsUnicode(false);

                entity.HasOne(d => d.Nationality)
                    .WithMany(p => p.OwnerIdentityDocuments)
                    .HasForeignKey(d => d.NationalityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IdentityDocuments_Nationalities");
            });

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.HasIndex(e => e.BusinessTypeId);

                entity.HasIndex(e => e.CreditApplicationSignedBy);

                entity.HasIndex(e => e.Id)
                    .HasName("IX_TradeLicenses_TradeName")
                    .IsUnique();

                entity.HasIndex(e => e.IssuancePlaceId);

                entity.HasIndex(e => e.PartnerTypeId);

                entity.HasIndex(e => e.PaymentTermId);

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.ApprovedBy).IsUnicode(false);

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.CreditApplicationSignedBy).IsUnicode(false);

                entity.Property(e => e.CreditDeptRemarks).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Fax).IsUnicode(false);

                entity.Property(e => e.LegalTitle).IsUnicode(false);

                entity.Property(e => e.LicenseNo).IsUnicode(false);

                entity.Property(e => e.Mobile).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.PaymentTermDisplay).IsUnicode(false);

                entity.Property(e => e.PostalCode).IsUnicode(false);

                entity.Property(e => e.ProductsRequired).IsUnicode(false);

                entity.Property(e => e.RatingRemarks).IsUnicode(false);

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.SunAccountCode).IsUnicode(false);

                entity.Property(e => e.SunAccountName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.TaxRegistrationNo).IsUnicode(false);

                entity.Property(e => e.Telephone).IsUnicode(false);

                entity.HasOne(d => d.BusinessType)
                    .WithMany(p => p.Partners)
                    .HasForeignKey(d => d.BusinessTypeId)
                    .HasConstraintName("FK_Partners_BusinessTypes");

                entity.HasOne(d => d.IssuancePlace)
                    .WithMany(p => p.Partners)
                    .HasForeignKey(d => d.IssuancePlaceId)
                    .HasConstraintName("FK_Partners_IssuancePlaces");

                entity.HasOne(d => d.ParentCompany)
                    .WithMany(p => p.InverseParentCompany)
                    .HasForeignKey(d => d.ParentCompanyId)
                    .HasConstraintName("FK_Partners_Partners");

                entity.HasOne(d => d.PartnerType)
                    .WithMany(p => p.Partners)
                    .HasForeignKey(d => d.PartnerTypeId)
                    .HasConstraintName("FK_Partners_PartnerLegalFormTypes");

                entity.HasOne(d => d.PaymentTerm)
                    .WithMany(p => p.Partners)
                    .HasForeignKey(d => d.PaymentTermId)
                    .HasConstraintName("FK_Partners_PartnerPaymentTerms");

                entity.HasOne(d => d.Rating)
                    .WithMany(p => p.Partners)
                    .HasForeignKey(d => d.RatingId)
                    .HasConstraintName("FK_Partners_PartnerRatings");

                entity.HasOne(d => d.SalesPerson)
                    .WithMany(p => p.Partners)
                    .HasForeignKey(d => d.SalesPersonId)
                    .HasConstraintName("FK_Partners_SalesPerson");
            });

            modelBuilder.Entity<PartnerAttachment>(entity =>
            {
                entity.HasIndex(e => e.AttachmentType)
                    .HasName("IX_PartnerAttachments_AttachmentTypeId");

                entity.HasIndex(e => e.PartnerId);

                entity.Property(e => e.AttachmentPath).IsUnicode(false);

                entity.Property(e => e.AttachmentType).IsUnicode(false);

                entity.Property(e => e.AttachmentVersion).IsUnicode(false);

                entity.Property(e => e.FollowUpHistory).IsUnicode(false);

                entity.Property(e => e.FollowUpNote).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.PartnerAttachments)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_PartnerAttachments_Partners");
            });

            modelBuilder.Entity<PartnerBanker>(entity =>
            {
                entity.HasIndex(e => e.BankId);

                entity.HasIndex(e => e.PartnerId);

                entity.Property(e => e.AccountNo).IsUnicode(false);

                entity.Property(e => e.BranchLocation).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.PartnerBankers)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PartnerBankers_Banks");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.PartnerBankers)
                    .HasForeignKey(d => d.PartnerId)
                    .HasConstraintName("FK_PartnerBankers_Partners");
            });

            modelBuilder.Entity<PartnerContact>(entity =>
            {
                entity.HasIndex(e => e.PartnerId);

                entity.Property(e => e.ContactType).IsUnicode(false);

                entity.Property(e => e.Designation).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.PartnerContacts)
                    .HasForeignKey(d => d.PartnerId)
                    .HasConstraintName("FK_PartnerContacts_Partners");
            });

            modelBuilder.Entity<PartnerGroup>(entity =>
            {
                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.Fax).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.PostalCode).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.Telephone).IsUnicode(false);

                entity.HasOne(d => d.PartnerGroupType)
                    .WithMany(p => p.PartnerGroups)
                    .HasForeignKey(d => d.PartnerGroupTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PartnerGroups_PartnerGroupTypes");

                entity.HasOne(d => d.PaymentTerm)
                    .WithMany(p => p.PartnerGroups)
                    .HasForeignKey(d => d.PaymentTermId)
                    .HasConstraintName("FK_PartnerGroups_PartnerPaymentTerms");

                entity.HasOne(d => d.SalesPerson)
                    .WithMany(p => p.PartnerGroups)
                    .HasForeignKey(d => d.SalesPersonId)
                    .HasConstraintName("FK_PartnerGroups_SalesPersons");
            });

            modelBuilder.Entity<PartnerGroupMapping>(entity =>
            {
                entity.Property(e => e.AccountCode).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.Tag).IsUnicode(false);

                entity.HasOne(d => d.AccountCodeNavigation)
                    .WithMany(p => p.PartnerGroupMappings)
                    .HasForeignKey(d => d.AccountCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PartnerGroupMappings_CustomersFromSunSystem");

                entity.HasOne(d => d.PartnerGroup)
                    .WithMany(p => p.PartnerGroupMappings)
                    .HasForeignKey(d => d.PartnerGroupId)
                    .HasConstraintName("FK_PartnerGroupMappings_PartnerGroups");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.PartnerGroupMappings)
                    .HasForeignKey(d => d.PartnerId)
                    .HasConstraintName("FK_PartnerGroupMappings_Partners");
            });

            modelBuilder.Entity<PartnerGroupType>(entity =>
            {
                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<PartnerLegalFormType>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_PartnerLegalFormTypes")
                    .IsUnique();

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<PartnerOwner>(entity =>
            {
                entity.HasKey(e => new { e.PartnerId, e.IdentityDocumentId })
                    .HasName("PK_PartnerOwners_1");

                entity.HasIndex(e => e.IdentityDocumentId);

                entity.HasIndex(e => e.PartnerId)
                    .HasName("IX_PartnerOwners_TradeLicenseId");

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.IdentityDocument)
                    .WithMany(p => p.PartnerOwners)
                    .HasForeignKey(d => d.IdentityDocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PartnerOwners_IdentityDocuments");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.PartnerOwners)
                    .HasForeignKey(d => d.PartnerId)
                    .HasConstraintName("FK_PartnerOwners_Partners");
            });

            modelBuilder.Entity<PartnerPaymentTerm>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedUser)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('System')");

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.PaymentTermsGroupCode).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<PartnerProduct>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_PartnerProducts")
                    .IsUnique();

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<PartnerProject>(entity =>
            {
                entity.HasIndex(e => new { e.PartnerId, e.Name })
                    .HasName("IX_PartnerProjects")
                    .IsUnique();

                entity.Property(e => e.MainContractor).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.ProjectEmployer).IsUnicode(false);

                entity.Property(e => e.ProjectLocation).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.EmirateOrCountry)
                    .WithMany(p => p.PartnerProjects)
                    .HasForeignKey(d => d.EmirateOrCountryId)
                    .HasConstraintName("FK_PartnerProjects_EmiratesOrCountries");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.PartnerProjects)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PartnerProjects_Partners");
            });

            modelBuilder.Entity<PartnerRating>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Color).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<PartnerTradeReference>(entity =>
            {
                entity.HasIndex(e => e.PartnerId);

                entity.HasIndex(e => e.TradeReferenceTypeId);

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.ContactNo).IsUnicode(false);

                entity.Property(e => e.Designation).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.PaymentStatus).IsUnicode(false);

                entity.Property(e => e.PaymentTerms).IsUnicode(false);

                entity.Property(e => e.Period).IsUnicode(false);

                entity.Property(e => e.PersonInterviewed).IsUnicode(false);

                entity.Property(e => e.ProductsPurchased).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.PartnerTradeReferences)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PartnerTradeReferences_Partners");

                entity.HasOne(d => d.TradeReferenceType)
                    .WithMany(p => p.PartnerTradeReferences)
                    .HasForeignKey(d => d.TradeReferenceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PartnerTradeReferences_TradeReferenceTypes");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedUser)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('System')");

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.Type).IsUnicode(false);
            });

            modelBuilder.Entity<PaymentMethodByTerm>(entity =>
            {
                entity.HasKey(e => new { e.PaymentTermId, e.PaymentMethodId })
                    .HasName("PK_PaymentMethodsByPaymentTerm");

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.PaymentMethodByTerms)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentMethodsByPaymentTerm_PaymentMethods");

                entity.HasOne(d => d.PaymentTerm)
                    .WithMany(p => p.PaymentMethodByTerms)
                    .HasForeignKey(d => d.PaymentTermId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentMetodByTerms_PaymentTerms");
            });

            modelBuilder.Entity<PaymentReceiptVoucher>(entity =>
            {
                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.AccountReceivable)
                    .WithMany(p => p.PaymentReceiptVouchers)
                    .HasForeignKey(d => d.AccountReceivableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentReceiptVouchers_AccountReceivables");

                entity.HasOne(d => d.ReceivedBy)
                    .WithMany(p => p.PaymentReceiptVouchers)
                    .HasForeignKey(d => d.ReceivedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentReceiptVouchers_PaymentReceivers");
            });

            modelBuilder.Entity<PaymentReceiver>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedUser)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('System')");

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<PaymentTerm>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedUser)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('System')");

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<PaymentTermsAll>(entity =>
            {
                entity.Property(e => e.AccountCode).IsUnicode(false);
            });

            modelBuilder.Entity<PaymentTermsWithPartnerId>(entity =>
            {
                entity.Property(e => e.AccountCode).IsUnicode(false);
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_ProductCategories")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<ProductSpecification>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Type).IsUnicode(false);
            });

            modelBuilder.Entity<ProductUnit>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_ProductUnits")
                    .IsUnique();

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<Quotation>(entity =>
            {
                entity.HasIndex(e => e.DeliveryPointId);

                entity.HasIndex(e => e.Name)
                    .HasName("IX_Quotations_QuotationRef")
                    .IsUnique();

                entity.HasIndex(e => e.PartnerId)
                    .HasName("IX_Quotations_CustomerId");

                entity.HasIndex(e => e.PaymentTermId);

                entity.HasIndex(e => e.SalesPersonId);

                entity.HasIndex(e => e.UniqueQuotationNo)
                    .IsUnique();

                entity.Property(e => e.AdditionalNotes).IsUnicode(false);

                entity.Property(e => e.ApprovedOrigins).IsUnicode(false);

                entity.Property(e => e.Attention).IsUnicode(false);

                entity.Property(e => e.BbsBy).IsUnicode(false);

                entity.Property(e => e.BbsChargeType)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('By Sheet')");

                entity.Property(e => e.BbsRevisionCharge).HasDefaultValueSql("((0))");

                entity.Property(e => e.Body).IsUnicode(false);

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.DeliveryLocation).IsUnicode(false);

                entity.Property(e => e.Emails).IsUnicode(false);

                entity.Property(e => e.EnquiryContact).IsUnicode(false);

                entity.Property(e => e.EnquiryRefNo).IsUnicode(false);

                entity.Property(e => e.EnquiryRemarks).IsUnicode(false);

                entity.Property(e => e.EnquiryType).IsUnicode(false);

                entity.Property(e => e.JobSite).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.NewBbsCharge).HasDefaultValueSql("((0))");

                entity.Property(e => e.Opening).IsUnicode(false);

                entity.Property(e => e.ProductSpecifications).IsUnicode(false);

                entity.Property(e => e.QuotationHeader).IsUnicode(false);

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.RemarksForFollowup).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.Subject).IsUnicode(false);

                entity.Property(e => e.UniqueQuotationNo).IsUnicode(false);

                entity.HasOne(d => d.ApprovedBy)
                    .WithMany(p => p.Quotations)
                    .HasForeignKey(d => d.ApprovedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Quotations_Approvers");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Quotations)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_Quotations_Currencies");

                entity.HasOne(d => d.DeliveryPoint)
                    .WithMany(p => p.Quotations)
                    .HasForeignKey(d => d.DeliveryPointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Quotations_DeliveryPoints");

                entity.HasOne(d => d.Division)
                    .WithMany(p => p.Quotations)
                    .HasForeignKey(d => d.DivisionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Quotations_Divisions");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.Quotations)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Quotations_Partners");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Quotations)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Quotations_PaymentMethods");

                entity.HasOne(d => d.PaymentTerm)
                    .WithMany(p => p.Quotations)
                    .HasForeignKey(d => d.PaymentTermId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Quotations_PaymentTerms");

                entity.HasOne(d => d.SalesPerson)
                    .WithMany(p => p.Quotations)
                    .HasForeignKey(d => d.SalesPersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Quotations_SalesPersons");

                entity.HasOne(d => d.TechnicalContact)
                    .WithMany(p => p.Quotations)
                    .HasForeignKey(d => d.TechnicalContactId)
                    .HasConstraintName("FK_Quotations_TechnicalContact");
            });

            modelBuilder.Entity<QuotationAttachment>(entity =>
            {
                entity.Property(e => e.AttachmentPath).IsUnicode(false);

                entity.Property(e => e.AttachmentType).IsUnicode(false);

                entity.Property(e => e.AttachmentVersion).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.Quotation)
                    .WithMany(p => p.QuotationAttachments)
                    .HasForeignKey(d => d.QuotationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_QuotationAttachments_Quotations");
            });

            modelBuilder.Entity<QuotationAttachmentType>(entity =>
            {
                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<QuotationContact>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedUser)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('System')");

                entity.Property(e => e.Designation).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Mobile).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.Salutation).IsUnicode(false);

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.QuotationContacts)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuotationContacts_Partners");
            });

            modelBuilder.Entity<QuotationEmail>(entity =>
            {
                entity.Property(e => e.Emails).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.StateTrigger).IsUnicode(false);

                entity.HasOne(d => d.Division)
                    .WithMany(p => p.QuotationEmails)
                    .HasForeignKey(d => d.DivisionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuotationEmails_Divisions");
            });

            modelBuilder.Entity<QuotationLine>(entity =>
            {
                entity.HasIndex(e => e.QuotationId);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.Unit).IsUnicode(false);

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.QuotationLines)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK_QuotationLines_ProductCategories");

                entity.HasOne(d => d.Quotation)
                    .WithMany(p => p.QuotationLines)
                    .HasForeignKey(d => d.QuotationId)
                    .HasConstraintName("FK_QuotationItems_Quotations");
            });

            modelBuilder.Entity<QuotationMiscProduct>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Unit).IsUnicode(false);

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.QuotationMiscProducts)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuotationMiscProducts_ProductCategories");
            });

            modelBuilder.Entity<QuotationSalesCondition>(entity =>
            {
                entity.HasIndex(e => e.QuotationId);

                entity.Property(e => e.AllowEdit).HasDefaultValueSql("((1))");

                entity.Property(e => e.Condition).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.ViewName).IsUnicode(false);

                entity.HasOne(d => d.Quotation)
                    .WithMany(p => p.QuotationSalesConditions)
                    .HasForeignKey(d => d.QuotationId)
                    .HasConstraintName("FK_QuotationSalesConditions_Quotations");
            });

            modelBuilder.Entity<QuotationStateLog>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.HasOne(d => d.Quotation)
                    .WithMany(p => p.QuotationStateLogs)
                    .HasForeignKey(d => d.QuotationId)
                    .HasConstraintName("FK_QuotationStateLogs_Quotations");
            });

            modelBuilder.Entity<Reminder>(entity =>
            {
                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.EntityName).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.ReminderNote).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<SalesCondition>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedUser)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('System')");

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.ViewName).IsUnicode(false);
            });

            modelBuilder.Entity<SalesOrder>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasName("IX_SalesOrders_UniqueCustomerRef")
                    .IsUnique();

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.CustomerRef).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.UniqueCustomerRef).IsUnicode(false);

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.SalesOrders)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesOrders_Partners");

                entity.HasOne(d => d.Quotation)
                    .WithMany(p => p.SalesOrders)
                    .HasForeignKey(d => d.QuotationId)
                    .HasConstraintName("FK_SalesOrders_Quotations");
            });

            modelBuilder.Entity<SalesOrderAttachment>(entity =>
            {
                entity.HasIndex(e => e.SalesOrderId);

                entity.Property(e => e.AttachmentPath).IsUnicode(false);

                entity.Property(e => e.AttachmentType).IsUnicode(false);

                entity.Property(e => e.AttachmentVersion).IsUnicode(false);

                entity.Property(e => e.FollowUpHistory).IsUnicode(false);

                entity.Property(e => e.FollowUpNote).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.SalesOrder)
                    .WithMany(p => p.SalesOrderAttachments)
                    .HasForeignKey(d => d.SalesOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesOrderAttachments_SalesOrders");
            });

            modelBuilder.Entity<SalesOrderAttachmentType>(entity =>
            {
                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<SalesOrderLine>(entity =>
            {
                entity.HasIndex(e => e.SalesOrderId);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.Unit).IsUnicode(false);

                entity.HasOne(d => d.SalesOrder)
                    .WithMany(p => p.SalesOrderLines)
                    .HasForeignKey(d => d.SalesOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesOrderLines_SalesOrders");
            });

            modelBuilder.Entity<SalesOrderQuotation>(entity =>
            {
                entity.HasKey(e => new { e.SalesOrderId, e.QuotationId });

                entity.HasOne(d => d.Quotation)
                    .WithMany(p => p.SalesOrderQuotations)
                    .HasForeignKey(d => d.QuotationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesOrderQuotations_Quotations");

                entity.HasOne(d => d.SalesOrder)
                    .WithMany(p => p.SalesOrderQuotations)
                    .HasForeignKey(d => d.SalesOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesOrderQuotations_SalesOrders");
            });

            modelBuilder.Entity<SalesPerson>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_SalesPersons")
                    .IsUnique();

                entity.Property(e => e.Abbr).IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedUser)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('System')");

                entity.Property(e => e.Designation).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Mobile).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<SalesPersonsByDivision>(entity =>
            {
                entity.HasKey(e => new { e.DivisionId, e.SalesPersonId });

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.Division)
                    .WithMany(p => p.SalesPersonsByDivisions)
                    .HasForeignKey(d => d.DivisionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesPersonsByDivision_Divisions");

                entity.HasOne(d => d.SalesPerson)
                    .WithMany(p => p.SalesPersonsByDivisions)
                    .HasForeignKey(d => d.SalesPersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesPersonsByDivision_SalesPersons");
            });

            modelBuilder.Entity<SalesTermsByProductCategory>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.SalesTermsByProductCategories)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesTermsByProductCategory_ProductCategories");
            });

            modelBuilder.Entity<SalesTermsMaster>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<SunAccountCodeGroupMapping>(entity =>
            {
                entity.HasKey(e => new { e.AccountCode, e.GroupId })
                    .HasName("PK_SunAccountCodeGroupMapping_1");

                entity.Property(e => e.AccountCode).IsUnicode(false);

                entity.HasOne(d => d.AccountCodeNavigation)
                    .WithMany(p => p.SunAccountCodeGroupMappings)
                    .HasForeignKey(d => d.AccountCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SunAccountCodeGroupMapping_CustomersFromSunSystem");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.SunAccountCodeGroupMappings)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_SunAccountCodeGroupMapping_SunAccountGroups");
            });

            modelBuilder.Entity<SunAccountGroup>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedUser)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.SunAccountGroups)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_SunAccountGroups_PartnerProjects");
            });

            modelBuilder.Entity<SunSystemAllocatedInvoice>(entity =>
            {
                entity.Property(e => e.AccountCode).IsUnicode(false);

                entity.Property(e => e.AccountName).IsUnicode(false);

                entity.Property(e => e.AccountType).IsUnicode(false);

                entity.Property(e => e.AccountingPeriod).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.DocumentNo).IsUnicode(false);

                entity.Property(e => e.JournalSource).IsUnicode(false);

                entity.Property(e => e.PaymentMethod).IsUnicode(false);

                entity.Property(e => e.PaymentTermGroupCode).IsUnicode(false);

                entity.HasOne(d => d.PaymentTerm)
                    .WithMany(p => p.SunSystemAllocatedInvoices)
                    .HasForeignKey(d => d.PaymentTermId)
                    .HasConstraintName("FK_SunSystemAllocatedInvoices_PartnerPaymentTerms");
            });

            modelBuilder.Entity<SunSystemDataFile>(entity =>
            {
                entity.Property(e => e.ImportedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<SunSystemUnAllocatedInvoice>(entity =>
            {
                entity.Property(e => e.AccountCode).IsUnicode(false);

                entity.Property(e => e.AccountName).IsUnicode(false);

                entity.Property(e => e.AccountType).IsUnicode(false);

                entity.Property(e => e.AccountingPeriod).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.DocumentNo).IsUnicode(false);

                entity.Property(e => e.JournalSource).IsUnicode(false);

                entity.Property(e => e.PaymentMethod).IsUnicode(false);

                entity.Property(e => e.PaymentTermGroupCode).IsUnicode(false);

                entity.HasOne(d => d.PaymentTerm)
                    .WithMany(p => p.SunSystemUnAllocatedInvoices)
                    .HasForeignKey(d => d.PaymentTermId)
                    .HasConstraintName("FK_SunSystemUnAllocatedInvoices_PartnerPaymentTerms");
            });

            modelBuilder.Entity<TaskAttachment>(entity =>
            {
                entity.Property(e => e.AttachmentPath).IsUnicode(false);

                entity.Property(e => e.AttachmentType).IsUnicode(false);

                entity.Property(e => e.AttachmentVersion).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskAttachments)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_TaskAttachments_UserTasks");
            });

            modelBuilder.Entity<TaskAttachmentType>(entity =>
            {
                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<TechnicalContact>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedUser)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('System')");

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<TestPaymentMethodGroupsByCustomer>(entity =>
            {
                entity.Property(e => e.CustomerCode).IsUnicode(false);

                entity.Property(e => e.CustomerName).IsUnicode(false);

                entity.Property(e => e.PaymentMethodGroupCode).IsUnicode(false);

                entity.Property(e => e.TaxRegistrationNo).IsUnicode(false);
            });

            modelBuilder.Entity<TradeReferenceType>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_TradeReferenceTypes")
                    .IsUnique();

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<UnitsByProductCategory>(entity =>
            {
                entity.HasKey(e => new { e.ProductCategoryId, e.UnitId });

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.UnitsByProductCategories)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnitsByProductCategory_ProductCategories");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.UnitsByProductCategories)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnitsByProductCategory_ProductUnits");
            });

            modelBuilder.Entity<UserTask>(entity =>
            {
                entity.Property(e => e.AssignedToUser).IsUnicode(false);

                entity.Property(e => e.Category).IsUnicode(false);

                entity.Property(e => e.CreatedUser).IsUnicode(false);

                entity.Property(e => e.EntityName).IsUnicode(false);

                entity.Property(e => e.FollowUp).IsUnicode(false);

                entity.Property(e => e.ModifiedUser).IsUnicode(false);

                entity.Property(e => e.Priority).IsUnicode(false);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.Status).IsUnicode(false);

                entity.Property(e => e.Subject).IsUnicode(false);
            });

            modelBuilder.Entity<VQuotationStatu>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vQuotationStatus");

                entity.Property(e => e.Division).IsUnicode(false);

                entity.Property(e => e.Emails).IsUnicode(false);

                entity.Property(e => e.JobSite).IsUnicode(false);

                entity.Property(e => e.PartnerName).IsUnicode(false);

                entity.Property(e => e.PayementTerm).IsUnicode(false);

                entity.Property(e => e.PaymentMethod).IsUnicode(false);

                entity.Property(e => e.PreparedBy).IsUnicode(false);

                entity.Property(e => e.QuotationRef).IsUnicode(false);

                entity.Property(e => e.SalesPerson).IsUnicode(false);
            });

            modelBuilder.HasSequence("CollectioVoucherSequence");

            modelBuilder.HasSequence("PaymentReceiptVoucherSequence").StartsAt(10001);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
