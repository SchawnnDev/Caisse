using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseReservationLibrary.Delegates;
using CaisseReservationLibrary.Packets;
using Networker.Common.Abstractions;

namespace CaisseReservationLibrary.Events
{
	public class ServerEvents
	{

		private static event ServerDelegates.DisconnectEvent _onDisconnect;

		public static event ServerDelegates.DisconnectEvent OnDisconnect
		{
			add => _onDisconnect += value;
			remove => _onDisconnect -= value;
		}

		private static event ServerDelegates.ConnectEvent _onConnect;

		public static event ServerDelegates.ConnectEvent OnConnect
		{
			add => _onConnect += value;
			remove => _onConnect -= value;
		}
	}
}
