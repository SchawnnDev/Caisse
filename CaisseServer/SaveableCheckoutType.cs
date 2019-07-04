using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Events;
using ProtoBuf;

namespace CaisseServer
{
	[ProtoContract]
    [Table("checkout_types")]
    public class SaveableCheckoutType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

		[ProtoMember(1)] public string Name { get; set; }

	    [ProtoMember(2)] public int Type { get; set; }

		[ProtoMember(3)] public SaveableEvent Event { get; set; }

        public override string ToString() => Name;
    }
}