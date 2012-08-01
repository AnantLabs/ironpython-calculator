using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Calculator.Properties;
using AutoUpdaterDotNET;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for DockWindow.xaml
    /// </summary>
    public partial class DockWindow : Window
    {
        public DockWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #if RELEASE
                AutoUpdater.Start("http://localhost/AutoUpdater.NET_AppCast.xml", true);
            #endif
            App.ConsoleWin = this.ConsoleWin;
            DockPlane.ToggleAutoHide();
            ApplySettings();
        }

        private void ApplySettings()
        {
            if (Settings.Default.WinLeft >= 0) this.Left = Settings.Default.WinLeft;
            if (Settings.Default.WinTop >= 0) this.Top = Settings.Default.WinTop;
            if (Settings.Default.Width > 1) this.Width = Settings.Default.Width;
            if (Settings.Default.Height > 1) this.Height = Settings.Default.Height;
        }

        private void SaveSettings()
        {
            Settings.Default.WinLeft = (int)this.Left;
            Settings.Default.WinTop = (int)this.Top;
            Settings.Default.Width = (int)this.Width;
            Settings.Default.Height = (int)this.Height;
            Settings.Default.Save();
        }

        private void CreateBrowser(string Url)
        {
            Browser b = new Browser();
            b.Url = Url;
            var index = DocPlane.Items.Add(b);
            DockPlane.SelectedIndex = index;
        }

        private void RunProgram(string path)
        {
            Process p = new Process();
            string upath = Environment.ExpandEnvironmentVariables(path);
            if (File.Exists(upath)) p.StartInfo.FileName = upath;
            else p.StartInfo.FileName = App.Core.AppDir + "\\" + upath;
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
            e.Cancel = false;
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MenuItem s = (MenuItem)sender;
            switch(s.Name)
            {
                case "HelpBuiltIn":
                    CreateBrowser("http://pycalc.webmaster442.hu/fuggvenyek-dokumentacioja/");
                    break;
                case "HelpPython":
                    CreateBrowser("http://pycalc.webmaster442.hu/python-dokumentacio/");
                    break;
                case "HelpWeb":
                    CreateBrowser("http://pycalc.webmaster442.hu/");
                    break;
            }
        }

        private void Tools_Click(object sender, RoutedEventArgs e)
        {
            MenuItem s = (MenuItem)sender;
            switch(s.Name)
            {
                case "ToolsGnuplot":
                    RunProgram(@"gnuplot\bin\wgnuplot.exe");
                    break;
                case "ToolsOsk":
                    RunProgram(@"%windir%\system32\osk.exe");
                    break;
            }
        }

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();
        }
    }
}
