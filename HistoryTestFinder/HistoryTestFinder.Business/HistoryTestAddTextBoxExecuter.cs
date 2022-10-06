using System;
using System.Windows.Input;

namespace HistoryTestFinder.Business
{
    public class HistoryTestAddTextBoxExecuter : ICommand
    {
        public HTFViewModel HTF { get; set; }

        public HistoryTestAddTextBoxExecuter(HTFViewModel htf)
        {
            HTF = htf;
        }

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
            HTF.TextBoxDataCollection.Add(new TestName() { TestNameTxt = "" });
        }
    }
}