using System;
using System.Windows;

namespace CaisseDesktop.Graphics.Admin.Events.Pages
{
    /// <summary>
    ///     Interaction logic for EventCheckoutTypePage.xaml
    /// </summary>
    public partial class EventCheckoutTypePage
    {
        public EventCheckoutTypePage()
        {
            InitializeComponent();
        }

        public override string CustomName => "EventCheckoutTypePage";

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
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
            return true;
        }

        public override bool CanBack()
        {
            return true;
        }
    }
}