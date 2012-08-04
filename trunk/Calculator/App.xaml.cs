using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using AvalonDock;
using Calculator.Classes;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static MainWindow ConsoleWin;
        internal static IpyCore Core;

        private static DockWindow DockWin
        {
            get { return (DockWindow)App.Current.MainWindow; }
        }

        internal static void SetTaskbarProgress(System.Windows.Shell.TaskbarItemProgressState State, double Value = 0)
        {
            DockWin.TaskBar.ProgressState = State;
            DockWin.TaskBar.ProgressValue = Value;
        }
    }
}
