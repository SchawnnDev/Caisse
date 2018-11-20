using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer.Events;

namespace CaisseServer
{
    [Table("Cashiers")]
    public class SaveableCashier
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Login { get; set; }

        public string FirstName { get; set; }

        public string Name { get; set; }

        public SaveableTimeSlot TimeSlot { get; set; }

        public bool WasHere { get; set; }

        /**
         *  If the cashier is missing 
         */
        
        public SaveableCashier Substitute { get; set; }

        public SaveableTimeSlot SubstituteTimeSlot { get; set; }

        public SaveableCashier()
        {
        }

    }
}
