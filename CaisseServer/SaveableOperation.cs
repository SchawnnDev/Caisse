using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Items;
using ProtoBuf;

namespace CaisseServer
{
	[ProtoContract]
	[Table("operations")]
	public class SaveableOperation
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ProtoMember(1)] public SaveableInvoice Invoice { get; set; }

		[ProtoMember(2)] public SaveableArticle Item { get; set; }

		[ProtoMember(3)] public int Amount { get; set; }

		[NotMapped] public decimal FinalPrice => Amount * Item.Price;

		[NotMapped] public bool IsEventItem { get; set; } = false;
	}
}