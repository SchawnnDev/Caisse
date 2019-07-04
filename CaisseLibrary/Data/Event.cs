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
	public struct Event
	{
		[ProtoMember(1)] public SaveableEvent SaveableEvent { get; set; }
		[ProtoMember(2)] public List<CheckoutType> CheckoutTypes { get; set; }
		[ProtoMember(3)] public List<SaveablePaymentMethod> PaymentMethods { get; set; }
		[ProtoMember(4)] public List<SaveableOwner> Owners { get; set; }
	}
}
