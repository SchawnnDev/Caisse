using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CaisseReservationServer
{
	public class UserSock
	{
		public UserSock(int nClientID, Socket s)
		{
			_iClientID = nClientID;
			_UserSocket = s;
			_dTimer = DateTime.Now;//Initialize the ping timer to the current time
			_szStationName = string.Empty;
			_szClientName = string.Empty;
			_UserListentingPort = 9998;//default
			_szAlternateIP = string.Empty;
			_pingStatClass = new PingStatsClass();
		}

		public int iClientID => _iClientID;
		public Socket UserSocket => _UserSocket;
		public DateTime dTimer
		{
			get => _dTimer;
			set => _dTimer = value;
		}
		public string szClientName
		{
			get => _szClientName;
			set => _szClientName = value;
		}
		public string szStationName
		{
			get => _szStationName;
			set => _szStationName = value;
		}
		public ushort UserListentingPort
		{
			get => _UserListentingPort;
			set => _UserListentingPort = value;
		}
		public string szAlternateIP
		{
			get => _szAlternateIP;
			set => _szAlternateIP = value;
		}
		public PingStatsClass PingStatClass
		{
			get => _pingStatClass;
			set => _pingStatClass = value;
		}


		public int ZeroDataCount { get; internal set; }

		private Socket _UserSocket;
		private DateTime _dTimer;
		private int _iClientID;
		private string _szClientName;
		private string _szStationName;
		private ushort _UserListentingPort;
		private string _szAlternateIP;
		private PingStatsClass _pingStatClass;
	}
}
