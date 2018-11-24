using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseServer
{
    [Table("invoices")]
    public class SaveableInvoice
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public SaveablePaymentMethod PaymentMethod { get; set; }

        public SaveableInvoice()
        {

        }

    }
}
