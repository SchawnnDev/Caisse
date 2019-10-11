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

        private readonly List<ICommand> Commands;

        public CommandHandler()
        {
            Commands = new List<ICommand>();
        }

        public void Register(ICommand command)
        {
            if(CommandExists(command.Name())) return;
            Commands.Add(command);
        }

        public void Unregister(string commandName)
        {
            var cmd = GetCommand(commandName);
            if (cmd == null) return;
            Commands.Remove(cmd);
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

        private bool CommandExists(string commandName) => Commands.Any(t => t.Name() == commandName);

        private ICommand GetCommand(string commandName) =>
            CommandExists(commandName) ? Commands.Single(t => t.Name() == commandName) : null;

        private void PrintHelp(params string[] args)
        {

            if (args.Length == 1 && CommandExists(args[0]))
            {
                PrintHelpMessage(GetCommand(args[0]));
            }
            else
            {

                foreach (var command in Commands)
                {
                    PrintHelpMessage(command);
                }

            }

            WriteQuery();

        }
        
        private void PrintHelpMessage(ICommand command)
        {
            Console.Write($"> {command.Name()}: ");
            command.PrintHelp();
        }

        private void Execute(string commandName, params string[] args)
        {
            if (commandName.Equals("help"))
            {
                PrintHelp(args);
                return;
            }

            var cmd = GetCommand(commandName);

            if (cmd == null)
            {
                Console.WriteLine("This command does not exist!");
                PrintHelp();
                return;
            }

            cmd.Process(args);
            WriteQuery();
        }

        private void WriteQuery()
        {
            Console.Write("CaisseServer: ");
        }

        public void Init()
        {
            Console.WriteLine("Command handler ist activated.");
            Console.WriteLine("If you need some help, just write help or help [command].");
            WriteQuery();
        }
    }
}
