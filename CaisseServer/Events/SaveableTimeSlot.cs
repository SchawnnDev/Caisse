using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaisseServer.Events
{

    [Table("TimeSlots")]
    public class SaveableTimeSlot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableDay Day { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public SaveableTimeSlot()
        {
        }
    }
}