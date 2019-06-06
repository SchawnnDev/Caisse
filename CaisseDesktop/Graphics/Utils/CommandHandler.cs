using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CaisseDesktop.Graphics.Utils
{
	public class CommandHandler : ICommand
	{
		private Action<object> _action;
		private bool _canExecute;

		/// <summary>
		/// Creates instance of the command handler
		/// </summary>
		/// <param name="action">Action to be executed by the command</param>
		/// <param name="canExecute">A bolean property to containing current permissions to execute the command</param>
		public CommandHandler(Action<object> action, bool canExecute)
		{
			_action = action;
			_canExecute = canExecute;
		}

		/// <inheritdoc />
		/// <summary>
		/// Wires CanExecuteChanged event 
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		/// <summary>
		/// Forcess checking if execute is allowed
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public bool CanExecute(object parameter) => _canExecute;

		public void Execute(object parameter)
		{
			_action( parameter);
		}
	}
}
