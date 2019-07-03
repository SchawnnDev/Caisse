using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Export;
using CaisseServer.Items;

namespace CaisseServer
{
    [Table("operations")]
    public class SaveableOperation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableInvoice Invoice { get; set; }

        public SaveableArticle Item { get; set; }

        public int Amount { get; set; }

        [NotMapped]
        public decimal FinalPrice => Amount * Item.Price;

        [NotMapped]
        public bool IsEventItem { get; set; } = false;

    }
}