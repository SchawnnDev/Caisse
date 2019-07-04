using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;
using CaisseServer.Events;
using CaisseServer.Items;
using ProtoBuf;

namespace CaisseLibrary.Data
{
	[ProtoContract]
	public struct CheckoutType : IData
	{
		[ProtoMember(1)] public SaveableCheckoutType SaveableCheckoutType;
		[ProtoMember(2)] public List<Checkout> Checkouts { get; set; }
		[ProtoMember(3)] public List<Article> Articles { get; set; }
		[ProtoMember(4)] public List<SaveableArticleEvent> ArticleEvents { get; set; }
		public void From<T>(T parent, CaisseServerContext context)
		{

			var saveableCheckoutType = parent as SaveableCheckoutType;

			SaveableCheckoutType = saveableCheckoutType;
			Checkouts = new List<Checkout>();
			Articles = new List<Article>();
			ArticleEvents = context.ArticleEvents.Where(t => t.Type.Id == saveableCheckoutType.Id).ToList();

			foreach (var saveableCheckout in context.Checkouts.Where(t => t.CheckoutType.Id == saveableCheckoutType.Id).Include(t=>t.Owner).Include(t=>t.Owner.Event).ToList())
			{
				var checkout = new Checkout();
				checkout.From(saveableCheckout, context);
				Checkouts.Add(checkout);
			}

			foreach (var saveableArticle in context.Articles.Where(t => t.Type.Id == saveableCheckoutType.Id).ToList())
			{
				var article = new Article();
				article.From(saveableArticle, context);
				Articles.Add(article);
			}

		}
	}
}
