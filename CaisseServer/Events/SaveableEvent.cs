using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Export;
using CaisseServer.Export.Exceptions;

namespace CaisseServer.Events
{
    [Table("events")]
    public class SaveableEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string AddressName { get; set; }

        public string Address { get; set; }

        public string PostalCodeCity { get; set; }

        public string Description { get; set; }

        public string ImageSrc { get; set; }

        public string Telephone { get; set; }

        public string Siret { get; set; }

    }
}