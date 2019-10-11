using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseReservationLibrary.Interfaces;

namespace CaisseReservationLibrary.Handlers
{
    public class CommandHandler
    {

        private readonly Dictionary<string, ICommand> Commands;

        public CommandHandler()
        {
            Commands = new Dictionary<string, ICommand>();
        }

        public void Register(string commandName, ICommand command)
        {
            if(Commands.ContainsKey(commandName))
                Commands[commandName] = command;
            else
                Commands.Add(commandName, command);
        }

        public void Unregister(string commandName)
        {
            if (Commands.ContainsKey(commandName))
                Commands.Remove(commandName);
        }

        public void Handle(string line)
        {

            if (string.IsNullOrWhiteSpace(line))
            {
                PrintHelp();
                return;
            }

            var split = line.Split(' ');

            switch (split.Length)
            {
                case 0:
                    PrintHelp();
                    break;
                case 1:
                    Execute(split[0]);
                    break;
                default:
                    Execute(split[0], split.Skip(1).ToArray());
                    break;
            }

        }


        private void PrintHelp(params string[] args)
        {

            if (args.Length != 1)
            {
                foreach (var command in Commands.Values)
                {
                    PrintHelpMessage(command);
                }
            }
            else
            {

                if (Commands.ContainsKey(args[0]))
                {
                    PrintHelpMessage(Commands[args[0]]);
                }
                else
                {
                    foreach (var command in Commands.Values)
                    {
                        PrintHelpMessage(command);
                    }
                }

            }

            WriteQuery();

        }

        private void PrintHelpMessage(ICommand command)
        {
            Console.Write("Help: ");
            command.PrintHelp();
        }

        private void Execute(string commandName, params string[] args)
        {
            if (!Commands.ContainsKey(commandName)) return;
            Commands[commandName].Process(args);
            WriteQuery();
        }

        private void WriteQuery()
        {
            Console.Write("CaisseServer: ");
        }
    }
}
