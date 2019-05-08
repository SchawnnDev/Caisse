using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CaisseReservationServer
{
	public class Packet
	{
		public Socket CurrentSocket;
		public int ClientNumber;
		public byte[] DataBuffer = new byte[1024];
		//public byte[] DataBuffer = new byte[4096];

		/// <summary>
		/// Construct a Packet Object
		/// </summary>
		/// <param name="sock">The socket this Packet is being used on.</param>
		/// <param name="client">The client number that this packet is from.</param>
		public Packet(Socket sock, int client)
		{
			CurrentSocket = sock;
			ClientNumber = client;
		}

	}

}
