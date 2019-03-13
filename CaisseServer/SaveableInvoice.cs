using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseIO;

namespace CaisseServer
{
    [Table("invoices")]
    public class SaveableInvoice : IExportable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public SaveableCashier Cashier { get; set; }

        public SaveablePaymentMethod PaymentMethod { get; set; }

        public object[] Export() => new object[]
        {
            "Invoice",
            Id,
            Date,
            Cashier.Export(),
            PaymentMethod.Export()
        };
    }
}