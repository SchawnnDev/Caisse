using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Events;
using CaisseServer.Export;
using CaisseServer.Export.Exceptions;

namespace CaisseServer.Items
{
    [Table("article_max_sell_numbers")]
    public class SaveableArticleMaxSellNumber : IImportable, IExportable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableDay Day { get; set; }

        public SaveableArticle Article { get; set; }

        public int Amount { get; set; }

        public void Import(object[] args)
        {
            if (args.Length != 5) throw new IllegalArgumentNumberException(5, "nombre max de vente");
            if (!args[0].ToString().ToLower().Equals("articlemaxsellnumber"))
                throw new TypeNotRecognisedException("nombre max de vente (ArticleMaxSellNumber)");

            Id = args[1] as int? ?? 0;
            Amount = args[4] as int? ?? 0;

            if (args[2] is SaveableDay day)
            {
                Day = day;
            }
            else
            {
                Day = new SaveableDay();
                Day.Import(args[2] as object[]);
            }

            if (args[3] is SaveableArticle article)
            {
                Article = article;
            }
            else
            {
                Article = new SaveableArticle();
                Article.Import(args[3] as object[]);
            }
        }

        public object[] Export() => new object[]
        {
            "ArticleMaxSellNumber",
            Id,
            Day.Export(),
            Article.Export(),
            Amount
        };
    }
}