using System;
using System.Threading;
using CaisseReservationLibrary;
using CaisseReservationLibrary.Handlers;
using CaisseReservationLibrary.Packets;
using Microsoft.Extensions.Logging;
using Networker.Extensions.ProtobufNet;
using Networker.Server;
using Networker.Server.Abstractions;

namespace CaisseReservationServer
{
	public class Program
	{

		private static IServer Server;

		static void Main(string[] args)
		{
			Server = new ServerBuilder()
				.UseTcp(5456)
				.UseUdp(5457) // todo : config
				.ConfigureLogging(loggingBuilder =>
				{
				//	loggingBuilder.AddConfiguration(config.GetSection("Logging")); make config
					loggingBuilder.AddConsole();
				})
				.UseProtobufNet()
				.RegisterPacketHandler<ArticleReservationPacket, ArticleReservationPacketHandler>()
				.Build();

			Server.Start();

			while (Server.Information.IsRunning)
			{
				Thread.Sleep(10000);
			}

			AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

		}

		static void CurrentDomain_ProcessExit(object sender, EventArgs e)
		{
			Server.Stop();
		}

	}
}
