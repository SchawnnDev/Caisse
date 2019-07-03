using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Events;
using CaisseServer.Export;
using CaisseServer.Export.Exceptions;

namespace CaisseServer
{
    [Table("checkout_types")]
    public class SaveableCheckoutType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

		public int Type { get; set; }

        public SaveableEvent Event { get; set; }

        public override string ToString() => Name;
    }
}