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

        public bool WasHere { get; set; }

        public SaveableCheckout Checkout { get; set; }

        public DateTime LastActivity { get; set; }

        public string GetFullName()
        {
            return $"{FirstName} {Name}";
        }

        public void Import(object[] args)
        {
            if (args.Length != 8) throw new IllegalArgumentNumberException(8, "caissier");
            if (!args[0].ToString().ToLower().Equals("cashier"))
                throw new TypeNotRecognisedException("caissier (Cashier)");

            Id = args[1] as int? ?? 0;
            Login = args[2] as string;
            FirstName = args[3] as string;
            Name = args[4] as string;
            WasHere = args[5] is bool b && b;

            if (args[6] is SaveableCheckout checkout)
            {
                Checkout = checkout;
            }
            else
            {
                Checkout = new SaveableCheckout();
                Checkout.Import(args[6] as object[]);
            }

            LastActivity = args[7] as DateTime? ?? new DateTime();
        }

        public object[] Export() => new object[]
        {
            "Cashier",
            Id,
            Login,
            FirstName,
            Name,
            WasHere,
            Checkout,
            LastActivity
        };
    }
}