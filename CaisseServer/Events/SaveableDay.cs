using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseIO;
using CaisseIO.Exceptions;

namespace CaisseServer.Events
{
    [Table("days")]
    public class SaveableDay : IImportable, IExportable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableEvent Event { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public void Import(object[] args)
        {

            if (args.Length != 5) throw new IllegalArgumentNumberException(5, "jour");
            if (!args[0].ToString().ToLower().Equals("day")) throw new TypeNotRecognisedException("jour (Day)");

            Id = args[1] is int i ? i : 0;
            Start = args[3] is DateTime dateTime ? dateTime : new DateTime();
            End = args[4] is DateTime time ? time : new DateTime();

            if (args[2] is SaveableEvent saveableEvent)
            {
                Event = saveableEvent;
            }
            else
            {
                Event = new SaveableEvent();
                Event.Import(args[2] as object[]);
            }

        }

        public object[] Export() => new object[]
        {
            "Day",
            Id,
            Event.Export(),
            Start,
            End
        };

    }
}