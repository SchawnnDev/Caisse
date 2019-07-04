using System.Collections.Generic;
using System.Linq;
using CaisseServer;
using ProtoBuf;

namespace CaisseLibrary.Data
{
	[ProtoContract]
    public struct Checkout : IData
	{
		[ProtoMember(1)] public SaveableCheckout SaveableCheckout { get; set; }
		[ProtoMember(2)] public List<CheckoutDay> Days { get; set; }

		public void From<T>(T parent, CaisseServerContext context)
		{
			var saveableCheckout = parent as SaveableCheckout;

			SaveableCheckout = saveableCheckout;
			Days = new List<CheckoutDay>();

			foreach (var saveableDay in context.Days.Where(t => t.Event.Id == saveableCheckout.Owner.Event.Id).ToList())
			{
				var day = new CheckoutDay();
				day.From(saveableDay, context);
				Days.Add(day);
			}

		}
	}
}