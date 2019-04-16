using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CaisseDesktop.Frontend.ViewModels;
using ReactiveUI;

namespace CaisseDesktop.Frontend.Views
{
	/// <summary>
	/// Interaction logic for SelectionView.xaml
	/// </summary>
	public partial class SelectionView : ReactiveWindow<SelectionViewModel>
	{
		public SelectionView()
		{
			InitializeComponent();
		}
	}
}
