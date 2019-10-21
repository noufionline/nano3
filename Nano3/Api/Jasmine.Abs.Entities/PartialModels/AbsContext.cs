// ReSharper disable once CheckNamespace

using Microsoft.EntityFrameworkCore;


// ReSharper disable once CheckNamespace
namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class AbsContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<AccountReceivable>()
                .Property(e => e.PaymentStatusId)
                .HasConversion(
                    v => (int)v,
                    v => (PaymentStatusTypes)v);

            modelBuilder
                .Entity<BankDocumentTransactionHistory>()
                .Property(e => e.PaymentStatusId)
                .HasConversion(
                    v => (int)v,
                    v => (PaymentStatusTypes)v);

            modelBuilder
               .Entity<AccountReceivable>()
               .Property(e => e.DocumentType)
               .HasConversion(
                   v => (int)v,
                   v => (AccountReceivableTypes)v);

            modelBuilder
                .Entity<CommercialInvoice>()
                .Property(e=> e.CommercialInvoiceStatus)
                .HasConversion(
                    v=> (int)v,
                    v=> (CommercialInvoiceStatusTypes)v);
        }
    }
}