using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Events;

namespace CaisseServer
{
    [Table("checkouts")]
    public class SaveableCheckout
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public SaveableOwner Owner { get; set; }

        public string Details { get; set; }

        public SaveableCheckoutType CheckoutType { get; set; }

        public SaveableEvent SaveableEvent { get; set; }
    }
}