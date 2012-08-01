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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Calculator.Classes;

namespace Calculator.Controls
{
    /// <summary>
    /// Interaction logic for ConstantPanel.xaml
    /// </summary>
    public partial class ConstantPanel : UserControl
    {
        private ConstantManager _consts;
        public event RoutedEventHandler ConstantButtonClick;

        public ConstantPanel()
        {
            InitializeComponent();
        }

        public double LookupConstant(string name)
        {
            return _consts[name];
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _consts = new ConstantManager();
            Button b;
            TextBlock content;
            foreach (var prefix in _consts.Prefixes)
            {
                b = new Button();
                content = new TextBlock();
                content.TextWrapping = TextWrapping.Wrap;
                content.Text = prefix;
                content.TextAlignment = TextAlignment.Center;
                b.Content = content;
                b.Style = (Style)FindResource("FunctionButton");
                b.Click += new RoutedEventHandler(b_Click);
                PrefixContainer.Children.Add(b);
            }

            foreach (var cons in _consts.Constants)
            {
                b = new Button();
                content = new TextBlock();
                content.TextWrapping = TextWrapping.Wrap;
                content.Text = cons;
                content.TextAlignment = TextAlignment.Center;
                b.Content = content;
                b.Style = (Style)FindResource("FunctionButton");
                b.Click += new RoutedEventHandler(b_Click);
                ConstContainer.Children.Add(b);
            }
        }

        void b_Click(object sender, RoutedEventArgs e)
        {
            if (ConstantButtonClick != null) ConstantButtonClick(sender, e);
        }
    }
}
