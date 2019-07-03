using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Export;
using CaisseServer.Export.Exceptions;

namespace CaisseServer.Events
{
    [Table("days")]
    public class SaveableDay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableEvent Event { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Color { get; set; }

    }
}