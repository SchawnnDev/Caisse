using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaisseServer.Events
{
    [Table("days")]
    public class SaveableDay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableCheckout Checkout { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}