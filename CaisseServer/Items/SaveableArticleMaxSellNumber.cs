using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Events;

namespace CaisseServer.Items
{
    [Table("article_max_sell_numbers")]
    public class SaveableArticleMaxSellNumber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableDay Day { get; set; }

        public SaveableArticle Article { get; set; }

        public int Amount { get; set; }

    }

}
