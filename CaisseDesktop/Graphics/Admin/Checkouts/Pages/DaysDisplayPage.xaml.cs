﻿using System;
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
    /// Interaction logic for DaysDisplayPage.xaml
    /// </summary>
    public partial class DaysDisplayPage
    {
        public DaysDisplayPage()
        {
            InitializeComponent();
        }

        public override void Update()
        {
        }

        public override void Add<T>(T item)
        {
        }

        public override bool CanClose() => true;

        public override bool CanBack() => true;

        public override string CustomName => "DaysDisplayPage";
    }
}
