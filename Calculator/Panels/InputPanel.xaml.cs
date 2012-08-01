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
using System.Globalization;
using Calculator.Controls;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for InputPanel.xaml
    /// </summary>
    public partial class InputPanel : DockableContent
    {
        public InputPanel()
        {
            InitializeComponent();
        }
        
        private string GetButtonText(object button)
        {
            if (button is FncButton)
            {
                FncButton s = (FncButton)button;
                return s.InsertText;
            }
            Button b = (Button)button;
            if (b.Content is TextBlock) return (b.Content as TextBlock).Text;
            else return (string)b.Content;
        }

        private void FunctionButtonAction(object Sender, RoutedEventArgs e)
        {
            string s = GetButtonText(Sender);
            if (App.ConsoleWin != null) App.ConsoleWin.InsertInputText(s + "(");
        }

        private void InputButtonAction(object Sender, RoutedEventArgs e)
        {
            string s = GetButtonText(Sender);
            if (App.ConsoleWin != null) App.ConsoleWin.InsertInputText(s);
        }

        private void ConstantPanel_ConstantButtonClick(object Sender, RoutedEventArgs e)
        {
            string s = GetButtonText(Sender);
            s = ConstantPanel.LookupConstant(s).ToString(new CultureInfo("En-US"));
            if (App.ConsoleWin != null) App.ConsoleWin.InsertInputText(s);
        }
    }
}
