using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseIO;
using CaisseIO.Exceptions;
using CaisseServer.Events;

namespace CaisseServer
{
    [Table("checkouts")]
    public class SaveableCheckout : IImportable, IExportable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public SaveableOwner Owner { get; set; }

        public string Details { get; set; }

        public SaveableCheckoutType CheckoutType { get; set; }

        public SaveableEvent SaveableEvent { get; set; }

        public object[] Export() => new object[]
        {
            "Checkout",
            Id,
            Name,
            Owner.Export(),
            Details,
            CheckoutType.Export(),
            SaveableEvent.Export()
        };

        public void Import(object[] args)
        {

            if(args.Length != 7 || !args[0].ToString().Equals("Checkout")) throw new TypeNotRecognisedException("Pas assez d'arguments pour importer cette caisse.");

            Id = args[1] is int i ? i : 0;
            Name = args[2] as string;
            Details = args[4] as string;

            if (args[3] is SaveableOwner owner)
            {
                Owner = owner;
            }
            else
            {
                Owner = new SaveableOwner();
                Owner.Import(args[3] as object[]);
            }

            if (args[5] is SaveableCheckoutType type)
            {
                CheckoutType = type;
            }
            else
            {
                CheckoutType = new SaveableCheckoutType();
                CheckoutType.Import(args[5] as object[]);
            }

            if (args[6] is SaveableEvent saveableEvent)
            {
                SaveableEvent = saveableEvent;
            }
            else
            {
                SaveableEvent = new SaveableEvent();
                SaveableEvent.Import(args[6] as object[]);
            }

        }
    }
}