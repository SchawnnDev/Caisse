using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaisseServer.Items
{
    [Table("items")]
    public class SaveableItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableCheckoutType Type { get; set; }

        public string Name { get; set; }

        public string ImageSrc { get; set; }

        public decimal Price { get; set; }
        
        public string Color { get; set; }

        public bool Cup { get; set; }

        public bool Active { get; set; }

        public bool NumberingTracking { get; set; }

        public int MaxSellNumberPerDay { get; set; }

    }
}