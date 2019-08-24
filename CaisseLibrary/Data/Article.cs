using System;
using System.Collections.Generic;
using System.Data.Entity;
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
//		[ProtoMember(1)] public SaveableArticle SaveableArticle { get; set; }

		[ProtoMember(1)] public string Id { get; set; }

		[ProtoMember(2)] public string Name { get; set; }

		[ProtoMember(3)] public string ImageSrc { get; set; }

		[ProtoMember(4)] public decimal Price { get; set; }

		[ProtoMember(5)] public int Position { get; set; }

		[ProtoMember(6)] public string Color { get; set; }

		[ProtoMember(7)] public bool NeedsCup { get; set; }

		[ProtoMember(8)] public bool Active { get; set; }

		[ProtoMember(9)] public bool NumberingTracking { get; set; }

		[ProtoMember(10)] public int MaxSellNumberPerDay { get; set; }

		[ProtoMember(11)] public List<ArticleMaxSellNumber> ArticleMaxSellNumbers { get; set; }

		public void From<T>(T parent, CaisseServerContext context)
		{
			var saveableArticle = parent as SaveableArticle;

			if (saveableArticle == null) return;

			Name = saveableArticle.Name;
			ImageSrc = saveableArticle.ImageSrc;
			Price = saveableArticle.Price;
			Position = saveableArticle.Position;
			Color = saveableArticle.Color;
			NeedsCup = saveableArticle.NeedsCup;
			Active = saveableArticle.Active;
			NumberingTracking = saveableArticle.NumberingTracking;
			MaxSellNumberPerDay = saveableArticle.MaxSellNumberPerDay;

			ArticleMaxSellNumbers = new List<ArticleMaxSellNumber>();

			foreach (var saveableArticleMaxSellNumber in context.ArticleMaxSellNumbers.Where(t => t.Article.Id == saveableArticle.Id)
				.Include(t=>t.Article).Include(t=>t.Day).ToList())
			{
				var articleMaxSellNumber = new ArticleMaxSellNumber();
				articleMaxSellNumber.From(saveableArticleMaxSellNumber, context);
				ArticleMaxSellNumbers.Add(articleMaxSellNumber);
			}
		}
	}
}
