using System;
using System.Collections.Generic;
using System.Text;

namespace CaisseReservationServer
{
	public class ReservationServer
	{

		private Server Server;

		public ReservationServer()
		{
			Server = new Server();
			Server.Listen(9998);
			Server.OnClientConnect += OnClientConnect;
			Server.OnClientDisconnect += OnClientDisconnect;
		}

		private void OnClientDisconnect(int clientnumber)
		{
		}

		private void OnClientConnect(int clientnumber)
		{
		}

		public void Stop()
		{
			Server.Stop();
		}
	}
}
