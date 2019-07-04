using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace CaisseServer.Items
{
	[ProtoContract]
	[Table("consigns")]
	public class SaveableConsign
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ProtoMember(1)] public SaveableInvoice Invoice { get; set; }

		[ProtoMember(2)] public int Amount { get; set; }
	}
}