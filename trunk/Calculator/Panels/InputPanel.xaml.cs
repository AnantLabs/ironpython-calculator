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

        public void CreatePanel(string Name)
        {
            TabItem tbitem = new TabItem();
            tbitem.Header = Name;
            ScrollViewer sw = new ScrollViewer();
            sw.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            WrapPanel container = new WrapPanel();
            container.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            sw.Content = container;
            tbitem.Content = sw;
            Tabs.Items.Add(tbitem);
        }

        public void AddButtonToPanel(string PanelName, string ButtonContent, string ButtonAction)
        {
            FncButton button = new FncButton();
            TextBlock content = new TextBlock();
            content.TextWrapping = TextWrapping.Wrap;
            content.Text = ButtonContent;
            content.TextAlignment = TextAlignment.Center;
            button.Content = content;
            button.InsertText = ButtonAction;
            button.Click += new RoutedEventHandler(FunctionButtonAction);
            button.Style = (Style)FindResource("FncButton");

            foreach (TabItem i in Tabs.Items)
            {
                if (i.Header.ToString() == PanelName)
                {
                    ScrollViewer sw = (ScrollViewer)i.Content;
                    (sw.Content as WrapPanel).Children.Add(button);
                    break;
                }
            }
        }
    }
}
