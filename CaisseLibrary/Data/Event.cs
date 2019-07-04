using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;
using CaisseServer.Events;
using ProtoBuf;

namespace CaisseLibrary.Data
{
	[ProtoContract]
	public struct Event : IData
	{
		[ProtoMember(1)] public SaveableEvent SaveableEvent { get; set; }
		[ProtoMember(2)] public List<CheckoutType> CheckoutTypes { get; set; }
		[ProtoMember(3)] public List<SaveablePaymentMethod> PaymentMethods { get; set; }
		[ProtoMember(4)] public List<SaveableOwner> Owners { get; set; }
		[ProtoMember(5)] public List<SaveableDay> Days { get; set; }

		public void From<T>(T parent, CaisseServerContext context)
		{
			var saveableEvent = parent as SaveableEvent;

			SaveableEvent = saveableEvent;
			CheckoutTypes = new List<CheckoutType>();
			PaymentMethods = context.PaymentMethods.Where(t => t.Event.Id == saveableEvent.Id).ToList();
			Owners = context.Owners.Where(t => t.Event.Id == saveableEvent.Id).ToList();
			Days = context.Days.Where(t => t.Event.Id == saveableEvent.Id).ToList();

			foreach (var checkoutType in context.CheckoutTypes.Where(t => t.Event.Id == saveableEvent.Id).ToList())
			{
				var type = new CheckoutType();
				type.From(checkoutType, context);
				CheckoutTypes.Add(type);
			}

		}
	}
}
