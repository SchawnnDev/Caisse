using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Events;
using CaisseServer.Export;
using CaisseServer.Export.Exceptions;

namespace CaisseServer
{
    [Table("payment_methods")]
    public class SaveablePaymentMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public decimal MinFee { get; set; }

        public SaveableEvent Event { get; set; } // needs a review.

    }
}