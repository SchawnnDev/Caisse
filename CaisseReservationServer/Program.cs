using System;
using System.Threading;
using CaisseReservationLibrary;
using CaisseReservationLibrary.Handlers;
using CaisseReservationLibrary.Packets;
using CaisseReservationServer.Commands;
using Microsoft.Extensions.Logging;
using Networker.Extensions.ProtobufNet;
using Networker.Server;
using Networker.Server.Abstractions;

namespace CaisseReservationServer
{
    public class Program
    {

        private static IServer Server;
        private static CommandHandler CommandHandler;

        static void Main(string[] args)
        {

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            Console.WriteLine("Starting Caisse Reservation Server V1.0...");

            Thread.Sleep(1000);

            Server = new ServerBuilder()
                .UseTcp(5456)
                .UseUdp(5457) // todo : config
                .ConfigureLogging(loggingBuilder =>
                {
                    //	loggingBuilder.AddConfiguration(config.GetSection("Logging")); make config
                    loggingBuilder.AddConsole();
                    loggingBuilder.SetMinimumLevel(
                        LogLevel.Debug);
                })
                .UseProtobufNet()
                .RegisterPacketHandler<ArticleReservationPacket, ArticleReservationPacketHandler>()
                .RegisterPacketHandler<string, MessagePacketHandler>()
                .Build();

            CommandHandler = new CommandHandler();

            CommandHandler.Register(new ClearCommand());
            CommandHandler.Register(new ExitCommand());

            Server.Start();

            new Thread(() =>
            {

                Thread.Sleep(2000);

                Start();

            }).Start();



        }

        private static void Start()
        {

            CommandHandler.Init();

            while (true)
            {
                if (!Server.Information.IsRunning) break;

                CommandHandler.Handle(Console.ReadLine());

            }
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Server.Stop();
        }

    }
}