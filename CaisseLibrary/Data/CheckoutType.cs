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
	public struct CheckoutType
	{
		[ProtoMember(1)] public SaveableCheckoutType SaveableCheckoutType;
		[ProtoMember(2)] public List<Checkout> Checkouts { get; set; }
		[ProtoMember(3)] public List<Article> Articles { get; set; }
		[ProtoMember(4)] public List<SaveableEvent> Events { get; set; }
	}
}
