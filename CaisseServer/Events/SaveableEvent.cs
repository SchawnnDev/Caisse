using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseIO;

namespace CaisseServer.Events
{
    [Table("events")]
    public class SaveableEvent : IImportable, IExportable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Addresse { get; set; }

        public string Description { get; set; }

        public string ImageSrc { get; set; }

        public void Import(object[] args)
        {
            throw new NotImplementedException();
        }

        public object[] Export()
        {
            throw new NotImplementedException();
        }
    }
}