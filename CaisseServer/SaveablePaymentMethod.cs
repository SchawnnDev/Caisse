using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseIO;
using CaisseIO.Exceptions;
using CaisseServer.Events;

namespace CaisseServer
{
    [Table("payment_methods")]
    public class SaveablePaymentMethod : IImportable, IExportable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public decimal MinFee { get; set; }

        public SaveableEvent Event { get; set; } // needs a review.

        public object[] Export() => new object[]
        {
            "PaymentMethod",
            Id,
            Name,
            Type,
            MinFee,
            Event.Export()
        };

        public void Import(object[] args)
        {
            if (args.Length != 6) throw new IllegalArgumentNumberException(6, "moyen de paiement");
            if (!args[0].ToString().ToLower().Equals("paymentmethod"))
                throw new TypeNotRecognisedException("moyen de paiement (PaymentMethod)");

            Id = args[1] as int? ?? 0;
            Name = args[2] as string;
            Type = args[3] as string;
            MinFee = args[4] as decimal? ?? 0;


            if (args[5] is SaveableEvent saveableEvent)
            {
                Event = saveableEvent;
            }
            else
            {
                Event = new SaveableEvent();
                Event.Import(args[5] as object[]);
            }
        }
    }
}