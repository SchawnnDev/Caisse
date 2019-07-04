using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;
using CaisseServer.Items;
using ProtoBuf;

namespace CaisseLibrary.Data
{
	[ProtoContract]
	public struct Article : IData
	{
		[ProtoMember(1)] public SaveableArticle SaveableArticle { get; set; }
		[ProtoMember(2)] public List<SaveableArticleMaxSellNumber> ArticleMaxSellNumbers { get; set; }
		public void From<T>(T parent, CaisseServerContext context)
		{
			var saveableArticle = parent as SaveableArticle;
			SaveableArticle = saveableArticle;
			ArticleMaxSellNumbers = context.ArticleMaxSellNumbers.Where(t => t.Article.Id == saveableArticle.Id).ToList();
		}
	}
}
