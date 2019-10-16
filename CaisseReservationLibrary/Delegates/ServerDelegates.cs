using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseReservationLibrary.Packets;
using Networker.Common.Abstractions;

namespace CaisseReservationLibrary.Delegates
{
	public class ServerDelegates
	{
		public delegate void DisconnectEvent(DisconnectPacket packet, IPacketContext context);
		public delegate void ConnectEvent(ConnectPacket packet, IPacketContext context);
	}
}
