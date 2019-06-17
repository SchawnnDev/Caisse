using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace CaisseReservationLibrary.Packets
{
	[ProtoContract] // Packet must be little as possible.
	public class ArticleReservationPacket
	{

		[ProtoMember(0)] public int ArticleId;
		[ProtoMember(1)] public int CheckoutId;
		[ProtoMember(2)] public int Number;

	}
}
