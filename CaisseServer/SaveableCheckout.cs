using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Events;
using ProtoBuf;

namespace CaisseServer
{
	[ProtoContract]
    [Table("checkouts")]
    public class SaveableCheckout
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

		[ProtoMember(1)] public string Name { get; set; }

		[ProtoMember(2)] public SaveableOwner Owner { get; set; }

		[ProtoMember(3)] public string Details { get; set; }

		[ProtoMember(4)] public SaveableCheckoutType CheckoutType { get; set; }

    }
}