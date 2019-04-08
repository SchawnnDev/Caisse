using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseIO.Exceptions;

namespace CaisseServer.Events
{
    [Table("time_slots")]
    public class SaveableTimeSlot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableDay Day { get; set; }

        public SaveableCheckout Checkout { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public SaveableCashier Cashier { get; set; }

        public SaveableSubstitute Substitute { get; set; }

        public bool Pause { get; set; }

        [NotMapped] public bool Blank { get; set; }

        public void Import(object[] args)
        {
            if (args.Length != 9) throw new IllegalArgumentNumberException(9, "créneau horaire");
            if (!args[0].ToString().ToLower().Equals("timeslot"))
                throw new TypeNotRecognisedException("créneau horaire (TimeSlot)");

            Id = args[1] as int? ?? 0;
            Start = args[4] is DateTime time ? time : new DateTime();
            End = args[5] is DateTime dateTime ? dateTime : new DateTime();
            Pause = args[8] is bool b && b;

            if (args[2] is SaveableDay day)
            {
                Day = day;
            }
            else
            {
                Day = new SaveableDay();
                Day.Import(args[2] as object[]);
            }

            if (args[3] is SaveableCheckout checkout)
            {
                Checkout = checkout;
            }
            else
            {
                Checkout = new SaveableCheckout();
                Checkout.Import(args[3] as object[]);
            }

            if (args[6] is SaveableCashier cashier)
            {
                Cashier = cashier;
            }
            else
            {
                Cashier = new SaveableCashier();
                Cashier.Import(args[6] as object[]);
            }

            if (args[7] is SaveableSubstitute substitute)
            {
                Substitute = substitute;
            }
            else
            {
                Substitute = new SaveableSubstitute();
                Substitute.Import(args[7] as object[]);
            }

        }

        public object[] Export() => new object[]
        {
            "TimeSlot",
            Id,
            Day,
            Checkout,
            Start,
            End,
            Pause,
        };
    }
}