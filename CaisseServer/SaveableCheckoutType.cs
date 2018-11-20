using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaisseServer
{
    [Table("CheckoutTypes")]
    public class SaveableCheckoutType
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public SaveableCheckoutType()
        {

        }

    }
}