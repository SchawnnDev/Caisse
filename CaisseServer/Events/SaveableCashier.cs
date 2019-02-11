using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Events;

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

        public SaveableTimeSlot TimeSlot { get; set; }

        public bool WasHere { get; set; }

        /**
         *  If the cashier is missing 
         */

        public SaveableCashier Substitute { get; set; }

        public SaveableTimeSlot SubstituteTimeSlot { get; set; }

        public string GetFullName()
        {
            return $"{FirstName} {Name}";
        }
    }
}