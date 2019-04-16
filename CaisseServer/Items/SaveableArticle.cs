using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseIO;
using CaisseIO.Exceptions;

namespace CaisseServer.Items
{
    [Table("articles")]
    public class SaveableArticle : IImportable, IExportable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageSrc { get; set; }

        public decimal Price { get; set; }

        public int Position { get; set; }

        public string Color { get; set; }

        //public bool Cup { get; set; }

        //public int ItemType { get; set; }
        public int ItemType { get; set; }

        public bool NeedsCup { get; set; }

        public bool Active { get; set; }

        public bool NumberingTracking { get; set; }

        public int MaxSellNumberPerDay { get; set; }

        public SaveableCheckoutType Type { get; set; }

        public void Import(object[] args)
        {
            if (args.Length != 13) throw new IllegalArgumentNumberException(13, "article");
            if (!args[0].ToString().ToLower().Equals("article"))
                throw new TypeNotRecognisedException("article (Article)");

            Id = args[1] as int? ?? 0;
            Name = args[2] as string;
            ImageSrc = args[3] as string;
            Price = args[4] as decimal? ?? 0;
            Position = args[5] as int? ?? 0;
            Color = args[6] as string;
            ItemType = args[7] as int? ?? 0;
            NeedsCup = args[8] as bool? ?? false;
            Active = args[9] as bool? ?? false;
            NumberingTracking = args[10] as bool? ?? false;
            MaxSellNumberPerDay = args[11] as int? ?? 0;

            if (args[12] is SaveableCheckoutType checkoutType)
            {
                Type = checkoutType;
            }
            else
            {
                Type = new SaveableCheckoutType();
                Type.Import(args[12] as object[]);
            }
        }

        public object[] Export() => new object[]
        {
            "Article",
            Id,
            Name,
            ImageSrc,
            Price,
            Position,
            Color,
            ItemType,
            NeedsCup,
            Active,
            NumberingTracking,
            MaxSellNumberPerDay,
            Type
        };
    }
}