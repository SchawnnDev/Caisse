using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace CaisseServer.Items
{
	[ProtoContract]
	[Table("article_events")]
	public class SaveableArticleEvent
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ProtoMember(1)] public SaveableCheckoutType Type { get; set; }

		[ProtoMember(2)] public SaveableArticle Item { get; set; }

		[ProtoMember(3)] public int NeededAmount { get; set; }

		[ProtoMember(4)] public SaveableArticle GivenItem { get; set; }

		[ProtoMember(5)] public int GivenAmount { get; set; }
	}
}