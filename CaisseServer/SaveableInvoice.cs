using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Export;
using CaisseServer.Items;

namespace CaisseServer
{
    [Table("invoices")]
    public class SaveableInvoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal GivenMoney { get; set; }

        public SaveableCashier Cashier { get; set; }

        public SaveablePaymentMethod PaymentMethod { get; set; }
    }
}