using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Export;
using CaisseServer.Export.Exceptions;

namespace CaisseServer.Events
{
    [Table("events")]
    public class SaveableEvent : IImportable, IExportable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string AddressName { get; set; }

        public string Address { get; set; }

        public string PostalCodeCity { get; set; }

        public string Description { get; set; }

        public string ImageSrc { get; set; }

        public string Telephone { get; set; }

        public string Siret { get; set; }


        public void Import(object[] args)
        {
            if (args.Length != 8) throw new IllegalArgumentNumberException(8, "événement");
            if (!args[0].ToString().ToLower().Equals("event"))
                throw new TypeNotRecognisedException("événement (Event)");

            Id = args[1] as int? ?? 0;
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