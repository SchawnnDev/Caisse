using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaisseServer.Items
{
    [Table("items")]
    public class SaveableArticle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableCheckoutType Type { get; set; }

        public string Name { get; set; }

        public string ImageSrc { get; set; }

        public decimal Price { get; set; }

        public int Position { get; set; }
        
        public string Color { get; set; }

        //public bool Cup { get; set; }

        //public int ItemType { get; set; }
        public string ItemType { get; set; }

        public bool NeedsCup { get; set; }

        public bool Active { get; set; }

        public bool NumberingTracking { get; set; }

        public int MaxSellNumberPerDay { get; set; }

    }
}