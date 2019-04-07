using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseIO;
using CaisseIO.Exceptions;
using CaisseServer.Events;

namespace CaisseServer
{
    [Table("cashiers")]
    public class SaveableCashier : IImportable, IExportable

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Login { get; set; }

        public string FirstName { get; set; }

        public string Name { get; set; }

        public SaveableTimeSlot TimeSlot { get; set; }

        public bool WasHere { get; set; }

		public DateTime LastConnection { get; set; }

        /**
         *  If the cashier is missing 
         */

        /* TODO

        public SaveableCashier Substitute { get; set; }

        public SaveableTimeSlot SubstituteTimeSlot { get; set; } */

        public string GetFullName()
        {
            return $"{FirstName} {Name}";
        }

        public void Import(object[] args)
        {
            if (args.Length != 7) throw new IllegalArgumentNumberException(7, "caissier");
            if (!args[0].ToString().ToLower().Equals("cashier"))
                throw new TypeNotRecognisedException("caissier (Cashier)");

            Id = args[1] as int? ?? 0;
            Login = args[2] as string;
            FirstName = args[3] as string;
            Name = args[4] as string;
            WasHere = args[6] is bool b && b;

            if (args[5] is SaveableTimeSlot slot)
            {
                TimeSlot = slot;
            }
            else
            {
                TimeSlot = new SaveableTimeSlot();
                TimeSlot.Import(args[5] as object[]);
            }
        }

        public object[] Export() => new object[]
        {
            "Cashier",
            Id,
            Login,
            FirstName,
            Name,
            TimeSlot.Export(),
            WasHere
        };
    }
}