using System;
using System.Collections.Generic;
using System.Text;
using CaisseReservationLibrary.Interfaces;
using CaisseReservationLibrary.Packets;

namespace CaisseReservationServer.Handlers
{
	public class UserHandler
	{

		private readonly List<ConnectPacket> _users;

		public UserHandler()
		{
			_users = new List<ConnectPacket>();
		}

		public bool Accept(byte[] password)
		{
			return false;
		}
	}
}
