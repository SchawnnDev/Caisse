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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CaisseDesktop.Graphics.Admin.Checkouts.Pages
{
    /// <summary>
    /// Interaction logic for CheckoutTimeTablePage.xaml
    /// </summary>
    public partial class CheckoutTimeTablePage
    {
        public CheckoutTimeTablePage()
        {
            InitializeComponent();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Add<T>(T item)
        {
            throw new NotImplementedException();
        }

        public override bool CanClose()
        {
            throw new NotImplementedException();
        }

        public override bool CanBack()
        {
            throw new NotImplementedException();
        }

        public override string CustomName => "CheckoutTimeTablePage";
    }
}
