using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace HistoryTestFinder.Business
{
	public class HTFViewModel : INotifyPropertyChanged
	{
		private ICommand _submitCommand;

		public ICommand SubmitCommand
		{
			get
			{
				if (_submitCommand == null)
					_submitCommand = new HistoryTestExecuter(this);
				return _submitCommand;
			}
			set
			{
				SetField(ref _submitCommand, value, "SubmitCommand");
			}
		}

		private ICommand _addTextBoxCommand;

		public ICommand AddTextBoxCommand
		{
			get
			{
				if (_addTextBoxCommand == null)
					_addTextBoxCommand = new HistoryTestAddTextBoxExecuter(this);
				return _addTextBoxCommand;
			}
			set
			{
				SetField(ref _addTextBoxCommand, value, "AddTextBoxCommand");
			}
		}

		private string _testName;

		public string TestName
		{
			get
			{
				if (_testName == null)
					_testName = "";
				return _testName;
			}
			set
			{
				SetField(ref _testName, value, "TestName");
			}
		}

		private ObservableCollection<TestName> _textBoxDataCollection;

		public ObservableCollection<TestName> TextBoxDataCollection
		{
			get
			{
				if (_textBoxDataCollection == null)
					_textBoxDataCollection = new ObservableCollection<TestName>() { new TestName() { TestNameTxt = "" } };
				return _textBoxDataCollection;
			}
			set
			{
				SetField(ref _textBoxDataCollection, value, "TextBoxDataCollection");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
		protected bool SetField<T>(ref T field, T value, string propertyName)
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return false;
			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}

	}
}
