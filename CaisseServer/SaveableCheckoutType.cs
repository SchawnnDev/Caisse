using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseIO;
using CaisseIO.Exceptions;
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

        public object[] Export() => new object[]
        {
            "CheckoutType",
            Name,
            Event.Export()
        };


        public void Import(object[] args)
        {
            if (args.Length != 3) throw new IllegalArgumentNumberException(3, "type de caisse");
            if (!args[0].ToString().ToLower().Equals("checkouttype"))
                throw new TypeNotRecognisedException("type de caisse (CheckoutType)");

            Id = args[1] as int? ?? 0;
            Name = args[2] as string;

            if (args[3] is SaveableEvent saveableEvent)
            {
                Event = saveableEvent;
            }
            else
            {
                Event = new SaveableEvent();
                Event.Import(args[3] as object[]);
            }
        }
    }
}