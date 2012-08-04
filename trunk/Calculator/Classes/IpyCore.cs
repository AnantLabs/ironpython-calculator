using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using Calculator.Controls;
using IronPython;
using IronPython.Hosting;
using IronPython.Runtime.Types;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using PluginApi;

namespace Calculator.Classes
{
    internal class IpyCore
    {
        private ScriptEngine _engine;
        private ScriptScope _scope;
        private List<string> _aliaslist;
        private CalculatorMode _mode;
        private List<string> _modeconstants;
        private NullStream _cstream;
        private EventRaisingStreamWriter _cwriter;
        private PythonSystax _syntax;

        public HistoryMan History { get; private set; }
        public IConsole ConsoleCont { get; set; }
        public TextBlock CalcMode { get; set; }
        public List<string> FilterList { get; private set; }

        public string AppDir
        {
            get
            {
                string ret = AppDomain.CurrentDomain.BaseDirectory;
                return ret;
            }
        }

        public PythonSystax Syntax
        {
            get { return _syntax; }
        }

        public string[] VariablesNames
        {
            get
            {
                return _scope.GetVariableNames().ToArray();
            }
        }

        public dynamic GetVariable(string name)
        {
            return _scope.GetVariable(name);
        }

        public void ClearVariables()
        {
            foreach (var v in _scope.GetVariableNames())
            {
                _scope.RemoveVariable(v);
            }
        }

        public void SetVariable(string name, object value)
        {
            try
            {
                _scope.SetVariable(name, value);
            }
            catch (Exception)
            {

            }
        }

        public IpyCore()
        {
            _mode = CalculatorMode.Deg;
            _aliaslist = new List<string>();
            _modeconstants = new List<string>();
            History = new HistoryMan();
        }

        public void Init()
        {
            Dictionary<String, Object> options = new Dictionary<string, object>();
            options["DivisionOptions"] = PythonDivisionOptions.New;

            _cstream = new NullStream();
            _cwriter = new EventRaisingStreamWriter(_cstream);
            _cwriter.StringWritten += new EventHandler<MyEvtArgs<string>>(_cwriter_StringWritten);

            _engine = Python.CreateEngine(options);
            _engine.Runtime.IO.SetOutput(_cstream, _cwriter);
            _engine.Runtime.IO.SetErrorOutput(_cstream, _cwriter);

            _scope = _engine.CreateScope();
            _scope.SetVariable("CalculatorMode", DynamicHelpers.GetPythonTypeFromType(typeof(PluginApi.CalculatorMode)));
            _scope.SetVariable("Calculator", DynamicHelpers.GetPythonTypeFromType(typeof(Calculator)));
            _scope.SetVariable("GnuPlot", DynamicHelpers.GetPythonTypeFromType(typeof(GnuPlot)));
            _scope.SetVariable("Ploter", new GnuPlot());
            FilterList = new List<string>();
            FilterList.Add("__builtins__");
            FilterList.Add("__file__");
            FilterList.Add("__name__");
            FilterList.Add("__doc__");
            LoadModules();
            _syntax = new PythonSystax();
            _syntax.Engine = this._engine;

            if (ConsoleCont == null) throw new ArgumentException("Console Window not set");

            if (File.Exists(AppDir + "\\welcome.rtf")) ConsoleCont.LoadRtf(AppDir + "\\welcome.rtf");
        }

        void _cwriter_StringWritten(object sender, MyEvtArgs<string> e)
        {
            ConsoleCont.BufferedWrite(e.Value);
        }

        private string NormalizeName(string name)
        {
            return name.Replace("`1", "");
        }

        private void LoadModules()
        {
            string[] modules = Directory.GetFiles(AppDir, "Calculator.*.dll");
            List<string> NameSpaces = new List<string>();
            bool loaded = false;
            ScriptSource loadcommand;

            foreach (var modul in modules)
            {
                loaded = false;
                try
                {
                    Assembly mod = Assembly.LoadFile(modul);
                    Type[] types = mod.GetTypes();

                    foreach (var t in types)
                    {
                        if (!t.IsPublic) continue;
                        Attribute[] attribs = Attribute.GetCustomAttributes(t);
                        try
                        {
                            //if (!t.IsAbstract || !t.IsSealed) continue; //static class check 
                            foreach (var attrib in attribs)
                            {
                                if (attrib is ReflectionLoad)
                                {
                                    if (!loaded) _engine.Runtime.LoadAssembly(mod);
                                    loaded = true;
                                    loadcommand = _engine.CreateScriptSourceFromString("from " + t.Namespace + " import " + NormalizeName(t.Name), SourceCodeKind.AutoDetect);
                                    FilterList.Add(t.Name);
                                    loadcommand.Execute(_scope);
                                    if (t.IsClass) GetMethods(t);
                                    break;
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            ConsoleCont.CurrentForeground = ConsoleCont.ErrorFontColor;
                            ConsoleCont.WriteLine("Error registering type: " + t.Name);
                            ConsoleCont.WriteLine("   Reason: " + ex.Message);
                            ConsoleCont.WriteLine("   Source:" + ex.Source);
                            ConsoleCont.CurrentForeground = ConsoleCont.DefaultFontColor;
                            continue;
                        }
                    }
                    foreach (var cmd in _aliaslist)
                    {
                        ScriptSource s = _engine.CreateScriptSourceFromString(cmd, SourceCodeKind.AutoDetect);
                        s.Execute(_scope);
                    }
                }
                catch (Exception ex)
                {
                    ConsoleCont.CurrentForeground = ConsoleCont.ErrorFontColor;
                    ConsoleCont.WriteLine("Error loading assembly " + modul);
                    ConsoleCont.WriteLine("   Reason: " + ex.Message);
                    ConsoleCont.WriteLine("   Source:" + ex.Source);
                    ConsoleCont.CurrentForeground = ConsoleCont.DefaultFontColor;
                    continue;
                }
            }
        }

        private void GetMethods(Type t)
        {
            char[] letters = "abcdefghijklmnopqrtuvwxyz".ToArray();
            MethodInfo[] members = t.GetMethods();
            string al = null, replace, panelname = null, action, content = null;
            StringBuilder aliascommand = new StringBuilder();
            StringBuilder Body = new StringBuilder();
            int calllength;
            bool panelfill = false, usealiases = false;

            Attribute[] attribs = Attribute.GetCustomAttributes(t);
            foreach (var atr in attribs)
            {
                if (atr is InputPanelGenerate)
                {
                    if (App.InputPanel != null)
                    {
                        panelname = (atr as InputPanelGenerate).PanelName;
                        App.InputPanel.CreatePanel(panelname);
                        usealiases = (atr as InputPanelGenerate).UseAliasNames;
                        panelfill = true;
                    }
                }
            }

            foreach (var member in members)
            {
                calllength = member.GetParameters().Length;
                object[] attrs = member.GetCustomAttributes(true);
                if (attrs.Length < 1) continue;
                foreach (var attr in attrs)
                {
                    if (attr is Alias)
                    {
                        aliascommand.Clear();
                        content = (attr as Alias).InputText;
                        al = (attr as Alias).AliasName;
                        if (string.IsNullOrEmpty(al)) break;
                        FilterList.Add(al);
                        replace = (t.FullName + "." + member.Name).Replace(t.Namespace + ".", "");
                        aliascommand.Append("def " + al);
                        Body.Clear();
                        Body.Append("(");
                        for (int i = 0; i < calllength; i++)
                        {
                            Body.Append(letters[i]);
                            if (i < calllength - 1) Body.Append(", ");
                        }
                        Body.Append(")");
                        aliascommand.Append(Body);
                        aliascommand.Append(":\n\t");
                        aliascommand.Append("return " + replace);
                        aliascommand.Append(Body);
                        _aliaslist.Add(aliascommand.ToString());
                        break;
                    }
                }
                if (panelfill && (al != null || content != null))
                {
                    if (usealiases) action = al;
                    else action = t.Name + "." + member.Name;
                    if (content == null) content = al;
                    App.InputPanel.AddButtonToPanel(panelname, content, action);
                    al = null;
                    content = null;
                }
            }
            PropertyInfo[] pi = t.GetProperties();
            foreach (var p in pi)
            {
                if (p.PropertyType == typeof(CalculatorMode))
                {
                    _modeconstants.Add((t.FullName + "." + p.Name).Replace(t.Namespace + ".", ""));
                }
            }
        }

        public void ChangeMode(string modestr = null)
        {
            string s = null;
            if (!string.IsNullOrEmpty(modestr))
            {
                switch (modestr.ToLower())
                {
                    case "rad":
                        _mode = CalculatorMode.Rad;
                        break;
                    case "grad":
                        _mode = CalculatorMode.Grad;
                        break;
                    case "deg":
                        _mode = CalculatorMode.Deg;
                        break;
                }
            }
            switch (_mode)
            {
                case CalculatorMode.Deg:
                    _mode = CalculatorMode.Rad;
                    break;
                case CalculatorMode.Rad:
                    _mode = CalculatorMode.Grad;
                    break;
                case CalculatorMode.Grad:
                    _mode = CalculatorMode.Deg;
                    break;
            }
            s = Enum.GetName(typeof(CalculatorMode), _mode).ToUpper();
            foreach (var i in _modeconstants)
            {
                try
                {
                    ScriptSource source = _engine.CreateScriptSourceFromString(i + " = CalculatorMode." + s, SourceCodeKind.AutoDetect);
                    source.Execute(_scope);
                }
                catch (Exception) { }
            }
            if (CalcMode != null)
            {
                if (CalcMode.Dispatcher.CheckAccess()) CalcMode.Text = s;
                else CalcMode.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        CalcMode.Text = s;
                    }));
            }
        }

        private static string FormatDouble(double input)
        {
            string gchar = CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
            string fchar = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            if (double.IsNaN(input) || double.IsInfinity(input)) return input.ToString();
            StringBuilder sb = new StringBuilder();
            bool passed = false;
            int j = 1;
            int i;
            char[] ar;
            string text = input.ToString();
            if (text.Contains(fchar))
            {
                for (i = text.Length - 1; i >= 0; i--)
                {
                    if (!passed && text[i] != fchar[0]) sb.Append(text[i]);
                    else if (text[i] == fchar[0])
                    {
                        sb.Append(text[i]);
                        passed = true;
                    }
                    if (passed && text[i] != fchar[0])
                    {
                        sb.Append(text[i]);
                        if (j % 3 == 0) sb.Append(gchar);
                        j++;
                    }
                }
                ar = sb.ToString().ToCharArray();
                Array.Reverse(ar);
                return new string(ar);
            }
            else
            {
                for (i = text.Length - 1; i >= 0; i--)
                {
                    sb.Append(text[i]);
                    if (j % 3 == 0) sb.Append(gchar);
                    j++;
                }
                ar = sb.ToString().ToCharArray();
                Array.Reverse(ar);
                return new string(ar);
            }
        }


        private string DisplayString(object o)
        {
            Type t = o.GetType();
            StringBuilder sb = new StringBuilder();
            switch (t.Name)
            {
                case "Byte":
                case "SByte":
                case "Int16":
                case "Int32":
                case "Int64":
                case "UInt16":
                case "UInt32":
                case "UInt64":
                case "Double":
                case "Single":
                    return FormatDouble(Convert.ToDouble(o));
                case "Complex":
                    Complex c = (Complex)o;
                    return string.Format("r: {0}; i: {1}; a: {2} rad; z: {3}", FormatDouble(c.Real), FormatDouble(c.Imaginary), FormatDouble(c.Phase), FormatDouble(c.Magnitude));
                default:
                    if (o is IEnumerable && t.Name != "String")
                    {
                        sb.Append("{ ");
                        foreach (var item in (IEnumerable)o)
                        {
                            sb.Append(item.ToString().Replace("\n", "\n   "));
                            sb.Append(", ");
                        }
                        sb.Append(" }");
                        return sb.ToString();
                    }
                    else return o.ToString().Replace("\n", "\n   ");
            }
        }

        public void Calculate(string Input)
        {
            ScriptSource source;
            object result;
            try
            {
                source = _engine.CreateScriptSourceFromString(Input, SourceCodeKind.AutoDetect);
                ConsoleCont.WriteLine(Input);
                result = source.Execute(_scope);
                if (result != null)
                {
                    ConsoleCont.CurrentForeground = ConsoleCont.OkFontColor;
                    ConsoleCont.WriteLine("Result: " + DisplayString(result));
                    ConsoleCont.CurrentForeground = ConsoleCont.DefaultFontColor;
                }
            }
            catch (Exception ex)
            {
                ConsoleCont.CurrentForeground = ConsoleCont.ErrorFontColor;
                ConsoleCont.WriteLine("Error: " + ex.Message + "\n");
                ConsoleCont.CurrentForeground = ConsoleCont.DefaultFontColor;
            }
        }

        public void LoadFileIntoContext(string filepath)
        {
            if (!File.Exists(filepath)) return;
            try
            {
                ScriptSource src = _engine.CreateScriptSourceFromFile(filepath);
                src.Execute(_scope);
            }
            catch (Exception ex)
            {
                ConsoleCont.CurrentForeground = ConsoleCont.ErrorFontColor;
                ConsoleCont.WriteLine("Error: " + ex.Message + "\n");
                ConsoleCont.CurrentForeground = ConsoleCont.DefaultFontColor;
            }
        }
    }
}
