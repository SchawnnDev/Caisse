using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Events;
using ProtoBuf;

namespace CaisseServer
{
	[ProtoContract]
    [Table("payment_methods")]
    public class SaveablePaymentMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

		[ProtoMember(1)] public string Name { get; set; }

		[ProtoMember(2)] public string Type { get; set; }

		[ProtoMember(3)] public decimal MinFee { get; set; }

		[ProtoMember(4)] public SaveableEvent Event { get; set; } // needs a review.

    }
}