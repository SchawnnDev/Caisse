using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseIO;
using CaisseServer.Items;

namespace CaisseServer
{
    [Table("operations")]
    public class SaveableOperation : IExportable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableInvoice Invoice { get; set; }

        public SaveableArticle Item { get; set; }

        public int Amount { get; set; }

        public decimal FinalPrice() => Amount * Item.Price;

        public object[] Export() => new object[]
        {
            "Operation",
            Id,
            Invoice.Export(),
            Item.Export(),
            Amount
        };

    }
}