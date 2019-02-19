using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Items;

namespace CaisseServer
{
    [Table("operations")]
    public class SaveableOperation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableArticle Item { get; set; }

        public int Amount { get; set; }

        public decimal FinalPrice() => Amount * Item.Price;

    }
}