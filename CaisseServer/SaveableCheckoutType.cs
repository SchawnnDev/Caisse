using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseIO;
using CaisseServer.Events;

namespace CaisseServer
{
    [Table("checkout_types")]
    public class SaveableCheckoutType : IImportable, IExportable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public SaveableEvent Event { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public object[] Export()
        {
            throw new System.NotImplementedException();
        }

        public void Import(object[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}