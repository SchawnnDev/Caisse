using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;
using CaisseServer.Items;
using ProtoBuf;

namespace CaisseLibrary.Data
{
	public struct ArticleMaxSellNumber : IData
	{
		[ProtoMember(1)] public int Id { get; set; }
		[ProtoMember(2)] public int ArticleId { get; set; }
		[ProtoMember(3)] public int DayId { get; set; }

		public void From<T>(T parent, CaisseServerContext context)
		{
			var saveableArticleMaxSellNumber = parent as SaveableArticleMaxSellNumber;
			Debug.Assert(saveableArticleMaxSellNumber != null, nameof(saveableArticleMaxSellNumber) + " != null");
			Id = saveableArticleMaxSellNumber.Id;
			ArticleId = saveableArticleMaxSellNumber.Article.Id;
			DayId = saveableArticleMaxSellNumber.Day.Id;
		}
	}
}
