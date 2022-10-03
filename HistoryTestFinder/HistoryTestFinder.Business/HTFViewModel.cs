using System;
using System.Windows.Input;

namespace HistoryTestFinder.Business
{
	public class HTFViewModel
	{
		private ICommand _submitCommand;

		public ICommand SubmitCommand
		{
			get
			{
				if (_submitCommand == null)
					_submitCommand = new Updater();
				return _submitCommand;
			}
			set
			{
				_submitCommand = value;
			}
		}
	}

	public class Updater : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute(object parameter)
		{

		}
	}
}
