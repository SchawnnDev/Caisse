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

        public void Import(object[] args)
        {

            if (args.Length != 8) throw new IllegalArgumentNumberException(8, "événement");
            if (!args[0].ToString().ToLower().Equals("Event")) throw new TypeNotRecognisedException("événement (Event)");

            Id = args[1] is int i ? i : 0;
            Name = args[2] as string;
            Start = args[3] is DateTime time ? time : new DateTime();
            End = args[4] is DateTime dateTime ? dateTime : new DateTime();
            Address = args[5] as string;
            Description = args[6] as string;
            ImageSrc = args[7] as string;

        }

        public object[] Export() => new object[]
        {
            "Event",
            Id,
            Name,
            Start,
            End,
            Address,
            Description,
            ImageSrc
        };

    }
}