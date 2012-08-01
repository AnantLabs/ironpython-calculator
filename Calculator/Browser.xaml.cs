using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AvalonDock;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for Browser.xaml
    /// </summary>
    public partial class Browser : DockableContent
    {
        public Browser()
        {
            InitializeComponent();
        }

        private void Navigate(string adress)
        {
            try
            {
                Browsercont.Navigate(adress);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string Url
        {
            get { return UrlBar.Text; }
            set
            {
                UrlBar.Text = value;
                Navigate(value);
            }
        }

        private void UrlBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Navigate(UrlBar.Text);
        }

        private void BrnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Browsercont.CanGoBack) Browsercont.GoBack();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (Browsercont.CanGoForward) Browsercont.GoForward();
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            Navigate("http://pycalc.webmaster442.hu");
        }
    }
}
