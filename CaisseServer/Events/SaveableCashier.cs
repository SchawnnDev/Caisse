using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Events;
using CaisseServer.Export;
using CaisseServer.Export.Exceptions;

namespace CaisseServer
{
    [Table("cashiers")]
    public class SaveableCashier

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Login { get; set; }

        public string FirstName { get; set; }

        public string Name { get; set; }

        public bool WasHere { get; set; }

        public bool Substitute { get; set; }

        public SaveableCheckout Checkout { get; set; }

        public DateTime LastActivity { get; set; }


        public string GetFullName() => $"{FirstName} {Name}";
    }
}