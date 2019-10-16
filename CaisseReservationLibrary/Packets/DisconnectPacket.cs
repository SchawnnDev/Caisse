using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseReservationLibrary.Enums;
using CaisseReservationLibrary.Handlers;
using ProtoBuf;

namespace CaisseReservationLibrary.Packets
{
	public class DisconnectPacket
	{
		[ProtoMember(1)] public int ComputerId;
		[ProtoMember(2)] public DisconnectReason Reason;
	}
}
