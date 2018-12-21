using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Events;

namespace CaisseServer
{
    [Table("checkout_types")]
    public class SaveableCheckoutType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public SaveableEvent Event { get; set; }

        public SaveableCheckoutType()
        {
        }

        public override string ToString() => Name;
    }
}