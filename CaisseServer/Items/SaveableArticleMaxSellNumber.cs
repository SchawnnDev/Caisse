using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Events;
using ProtoBuf;

namespace CaisseServer.Items
{
	[ProtoContract]
	[Table("article_max_sell_numbers")]
	public class SaveableArticleMaxSellNumber
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ProtoMember(1)] public SaveableDay Day { get; set; }

		[ProtoMember(2)] public SaveableArticle Article { get; set; }

		[ProtoMember(3)] public int Amount { get; set; }
	}
}