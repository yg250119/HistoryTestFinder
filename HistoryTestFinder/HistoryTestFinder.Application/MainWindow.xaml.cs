using HistoryTestFinder.Business;
using System.Windows;

namespace HistoryTestFinder.Application
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = new HTFViewModel();
		}
	}
}
