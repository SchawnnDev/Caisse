using System;
using System.Collections.Generic;
using System.Text;
using CaisseReservationLibrary.Handlers;
using CaisseReservationLibrary.Packets;
using Microsoft.Extensions.Logging;
using Networker.Extensions.ProtobufNet;
using Networker.Server;
using Networker.Server.Abstractions;

namespace CaisseReservationServer.Handlers
{
	public class ServerHandler
	{

		private readonly IServer Server;
		private readonly UserHandler UserHandler;

		public ServerHandler(int tcpPort, int udpPort)
		{
			UserHandler = new UserHandler();
			UserHandler = new UserHandler();

			// register handlers

			var builder = new ServerBuilder().UseTcp(tcpPort).UseUdp(udpPort).ConfigureLogging(loggingBuilder =>
			{
				//	loggingBuilder.AddConfiguration(config.GetSection("Logging")); make config
				loggingBuilder.AddConsole();
				loggingBuilder.SetMinimumLevel(LogLevel.Debug);
			}).UseProtobufNet();

			



			Server = builder.Build();
		}


		public void Start()
		{

			Server.Start();
		}

		public void Stop()
		{
			Server.Stop();
		}

	}
}
