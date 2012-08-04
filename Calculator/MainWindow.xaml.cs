using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using AvalonDock;
using Calculator.Classes;
using System.IO;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DocumentContent
    {
        private Thread _worker;
        private string _input;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.Core = new IpyCore();
            App.Core.ConsoleCont = Console;
            App.Core.CalcMode = CalcMode;
            App.Core.Init();
            InputDisplay.SyntaxLexer = App.Core.Syntax;
            HistoryView.ItemsSource = App.Core.History.ItemBind;
        }

        public void InsertInputText(string text)
        {
            int caret = InputDisplay.CaretIndex;
            InputDisplay.Text = InputDisplay.Text.Insert(InputDisplay.CaretIndex, text);
            InputDisplay.CaretIndex = (caret + text.Length);
        }

        private void WorkerFunction()
        {
            this.Dispatcher.Invoke((Action)delegate
            {
                WorkDisplay.Visibility = System.Windows.Visibility.Visible;
                WorkDisplay.IsEnabled = true;
                InputDisplay.Visibility = System.Windows.Visibility.Collapsed;
                InputDisplay.IsEnabled = false;
                App.Core.History.AddItem(_input);
                App.SetTaskbarProgress(System.Windows.Shell.TaskbarItemProgressState.Indeterminate);
            }, null);
            App.Core.Calculate(_input);
            this.Dispatcher.Invoke((Action)delegate
            {
                WorkDisplay.Visibility = System.Windows.Visibility.Collapsed;
                WorkDisplay.IsEnabled = false;
                InputDisplay.Visibility = System.Windows.Visibility.Visible;
                InputDisplay.IsEnabled = true;
                Console.WriteBuffer();
                InputDisplay.Text = "";
                InputDisplay.Focus();
                App.SetTaskbarProgress(System.Windows.Shell.TaskbarItemProgressState.None);
            }, null);
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            _input = InputDisplay.Text;
            _worker = new Thread(WorkerFunction);
            _worker.SetApartmentState(ApartmentState.STA);
            _worker.Start();
        }

        private void ClrButton_Click(object sender, RoutedEventArgs e)
        {
            InputDisplay.Text = "";
        }

        private void ModeChange_Click(object sender, RoutedEventArgs e)
        {
            App.Core.ChangeMode();
        }

        private void InputDisplay_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control) return;
            switch (e.Key)
            {
                case Key.Enter:
                    ExecuteButton_Click(sender, new RoutedEventArgs());
                    e.Handled = true;
                    break;
                case Key.Up:
                    InputDisplay.Text = App.Core.History.Previous();
                    e.Handled = true;
                    break;
                case Key.Down:
                    InputDisplay.Text = App.Core.History.Next();
                    e.Handled = true;
                    break;
            }
        }

        private void HistoryView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (HistoryView.SelectedItem == null) return;
            string text = (string)HistoryView.SelectedItem;
            InputDisplay.Text = text;
            InputTabs.SelectedIndex = 0;
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog Ofd = new System.Windows.Forms.OpenFileDialog();
            Ofd.Filter = "Python files|*.py";
            Ofd.Multiselect = true;
            if (Ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var file in Ofd.FileNames)
                {
                    Console.Write("Loading: " + System.IO.Path.GetFileName(file));
                    App.Core.LoadFileIntoContext(file);
                }
            }
        }

        private void BtnSaveOutput_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog Sfd = new System.Windows.Forms.SaveFileDialog();
            Sfd.Filter = "Rich text files|*.rtf";
            Sfd.AddExtension = true;
            if (Sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    if (File.Exists(Sfd.FileName)) File.Delete(Sfd.FileName);
                    Console.SaveRtf(Sfd.FileName);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("File write error", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


    }
}
