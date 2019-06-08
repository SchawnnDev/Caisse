using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseServer.Items
{

    [Table("article_events")]
    public class SaveableArticleEvent
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableCheckoutType Type { get; set; }

        public SaveableArticle Item { get; set; }

        public int NeededAmount { get; set; }

        public SaveableArticle GivenItem { get; set; }

        public int GivenAmount { get; set; }


    }
}
