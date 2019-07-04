using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer.Items;
using ProtoBuf;

namespace CaisseLibrary.Data
{
	[ProtoContract]
	public struct Article
	{
		[ProtoMember(1)] public SaveableArticle SaveableArticle { get; set; }
		[ProtoMember(2)] public List<SaveableArticleMaxSellNumber> ArticleMaxSellNumbers { get; set; }
	}
}
