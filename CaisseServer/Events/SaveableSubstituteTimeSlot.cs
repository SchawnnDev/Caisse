using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseIO.Exceptions;

namespace CaisseServer.Events
{
    [Table("substitute_time_slots")]
    public class SaveableSubstituteTimeSlot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableTimeSlot TimeSlot { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool Substitute { get; set; }

        public void Import(object[] args)
        {
            if (args.Length != 6) throw new IllegalArgumentNumberException(7, "créneau horaire de remplacement");
            if (!args[0].ToString().ToLower().Equals("subsitutetimeslot"))
                throw new TypeNotRecognisedException("créneau horaire de remplacement (TimeSlot)");

            Id = args[1] as int? ?? 0;
            Start = args[3] is DateTime time ? time : new DateTime();
            End = args[4] is DateTime dateTime ? dateTime : new DateTime();
            Substitute = args[5] is bool b && b;

            if (args[2] is SaveableTimeSlot timeSlot)
            {
                TimeSlot = timeSlot;
            }
            else
            {
                TimeSlot = new SaveableTimeSlot();
                TimeSlot.Import(args[2] as object[]);
            }

        }

        public object[] Export() => new object[]
        {
            "SubstituteTimeSlot",
            Id,
            TimeSlot,
            Start,
            End,
            Substitute
        };
    }
}