using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using AvalonDock;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for VariablesPanel.xaml
    /// </summary>
    public partial class VariablesPanel : DockableContent
    {
        public ObservableCollection<string> ListItems { get; private set; }

        public VariablesPanel()
        {
            InitializeComponent();
            ListItems = new ObservableCollection<string>();
            VariableList.ItemsSource = ListItems;
        }

        public void UpdateCollection()
        {
            if (UserOnly.IsChecked == false)
            {
                if (ListItems.Count == App.Core.VariablesNames.Length) return;
                ListItems.Clear();
                Array.ForEach(App.Core.VariablesNames, ListItems.Add);
            }
            else
            {
                
                var tmp = App.Core.VariablesNames.Except(App.Core.FilterList).AsParallel().ToArray();
                if (ListItems.Count == tmp.Length) return;
                ListItems.Clear();
                Array.ForEach(tmp, ListItems.Add);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            UpdateCollection();
        }
    }
}
