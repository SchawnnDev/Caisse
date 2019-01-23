using System.Data.Entity;
using CaisseServer.Events;
using CaisseServer.Items;

namespace CaisseServer
{
    public class CaisseServerContext : DbContext
    {
        public CaisseServerContext() : base("CaisseServer")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<SaveableCheckout> Checkouts { get; set; }
        public DbSet<SaveableCheckoutType> CheckoutTypes { get; set; }
        public DbSet<SaveablePaymentMethod> PaymentMethods { get; set; }
        public DbSet<SaveableOperation> Operations { get; set; }
        public DbSet<SaveableItem> Items { get; set; }
        public DbSet<SaveableInvoice> Invoices { get; set; }

        // Events

        public DbSet<SaveableCashier> Cashiers { get; set; }
        public DbSet<SaveableDay> Days { get; set; }
        public DbSet<SaveableEvent> Events { get; set; }
        public DbSet<SaveableTimeSlot> TimeSlots { get; set; }
        public DbSet<SaveableOwner> Owners { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}