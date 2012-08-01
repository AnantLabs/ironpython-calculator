using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Calculator.Controls
{
    class FncButton : Button
    {
        public static DependencyProperty InsertTextProperty = DependencyProperty.Register("InsertText", typeof(string), typeof(FncButton), new PropertyMetadata(""));

        public string InsertText
        {
            set { SetValue(InsertTextProperty, value); }
            get { return (string)GetValue(InsertTextProperty); }
        }
    }
}
