using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using IronPython.Runtime;

namespace Calculator.Classes
{
    public class GnuPlot : IDisposable
    {
        private bool _running
        {
            get
            {
                if (_gnuplot != null) return _gnuplot.Id > 0;
                else return false;
            }
        }

        private Process _gnuplot;
        private string _temp;
        private CultureInfo _us;

        private void ResetTerminal()
        {
            Write("set terminal wxt\n");
        }
        
        private void Write(string format, params object[] args)
        {
            if (!_running) return;
            _gnuplot.StandardInput.WriteLine(string.Format(format, args));
        }

        private void DataFileFromLists(List x, List y, string file = "")
        {
            if (string.IsNullOrEmpty(file)) file = _temp;
            if (File.Exists(file)) File.Delete(file);
            StreamWriter tw = File.CreateText(file);
            int limit = Math.Min(x.Count, y.Count);
            string xval, yval;
            for (int i = 0; i < limit; i++)
            {
                if (x[i] is IConvertible) xval = (x[i] as IConvertible).ToString(_us);
                else xval = x[i].ToString();
                if (y[i] is IConvertible) yval = (y[i] as IConvertible).ToString(_us);
                else yval = y[i].ToString();
                tw.Write(string.Format("{0}\t{1}\n", xval, yval));
            }
            tw.Close();
        }

        private void DataFileFromDict(PythonDictionary dict, string file = "")
        {
            if (string.IsNullOrEmpty(file)) file = _temp;
            if (File.Exists(file)) File.Delete(file);
            StreamWriter tw = File.CreateText(file);
            string xval, yval;
            foreach (var item in dict)
            {
                if (item.Key is IConvertible) xval = (item.Key as IConvertible).ToString(_us);
                else xval = item.Key.ToString();
                if (item.Value is IConvertible) yval = (item.Value as IConvertible).ToString(_us);
                else yval = item.Value.ToString();
                tw.Write(string.Format("{0}\t{1}\n", xval, yval));
            }
            tw.Close();
        }

        public GnuPlot()
        {
            _gnuplot = null;
            _temp = Path.GetTempFileName();
            _us = CultureInfo.CreateSpecificCulture("en-US");
        }

        public void Init()
        {
            if (_running) return;
            _gnuplot = new Process();
            _gnuplot.StartInfo.FileName = App.Core.AppDir + "\\gnuplot\\bin\\gnuplot.exe";
            _gnuplot.StartInfo.Arguments = "-p";
            _gnuplot.StartInfo.CreateNoWindow = true;
            _gnuplot.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            _gnuplot.StartInfo.UseShellExecute = false;
            _gnuplot.StartInfo.RedirectStandardInput = true;
            _gnuplot.Start();
            Thread.Sleep(500);
        }

        public void Stop()
        {
            if (File.Exists(_temp)) File.Delete(_temp);
            if (!_running) return;
            _gnuplot.Kill();
        }

        public void Plot(List x, List y, string title = "", string plottype = "lines")
        {
            Init();
            DataFileFromLists(x, y);
            Write("plot '{1}' with {0} using 1:2 ti '{2}'\n", plottype, _temp, title);
        }

        public void Plotf(List x, List y, string file)
        {
            Init();
            if (string.IsNullOrEmpty(file)) throw new ArgumentException("Invalid filename");
            DataFileFromLists(x, y, file);
        }

        public void Plot(PythonDictionary dict, string title = "", string plottype = "lines")
        {
            Init();
            DataFileFromDict(dict);
            Write("plot '{1}' with {0} using 1:2 ti '{2}'\n", plottype, _temp, title);
        }

        public void Plotf(PythonDictionary dict, string file)
        {
            Init();
            if (string.IsNullOrEmpty(file)) throw new ArgumentException("Invalid filename");
            DataFileFromDict(dict, file);
        }

        public void Plot(string source, string plottype = "lines")
        {
            Init();
            TextReader tx = File.OpenText(source);
            Write("plot {1} with {0} using 1:2\n", plottype, source);
            Write(tx.ReadToEnd());
            tx.Close();
        }

        public void Replot()
        {
            Write("replot\n");
        }

        public void SetTitle(string value)
        {
            Write("set title \"{0}\"\n", value);
            Replot();
        }

        public void SetXLabel(string value)
        {

            Write("set xlabel \"{0}\"\n", value);
            Replot();
        }

        public void SetYLabel(string value)
        {
            Write("set ylabel \"{0}\"\n", value);
            Replot();
        }

        public void Autoscale(bool value)
        {
            if (value) Write("set autoscale\n");
            else Write("unset autoscale\n");
            Replot();
        }

        public void SetXScale(double minimum, double maximum)
        {
            Write("set xrange[{0}:{1}]\n", minimum.ToString(_us), maximum.ToString(_us));
            Replot();
        }

        public void SetYScale(double minimum, double maximum)
        {
            Write("set yrange[{0}:{1}]\n", minimum.ToString(_us), maximum.ToString(_us));
            Replot();
        }

        public void SetLogScale(bool x, bool y)
        {
            if (x) Write("set logscale x\n");
            else Write("unset logscale x\n");
            if (y) Write("set logscale y\n");
            else Write("unset logscale y\n");
            Replot();
        }

        public void SavePng(string file, int width = 800, int height = 600)
        {
            if (string.IsNullOrEmpty(file)) throw new ArgumentException("Invalid filename");
            if (width < 0) throw new ArgumentException("Invalid width");
            if (height < 0) throw new ArgumentException("Invalid height");
            Write("set terminal pngcairo size {0}, {1}\n", width, height);
            Write("set output '{0}'\n", file);
            Replot();
            ResetTerminal();
        }

        public void SaveSvg(string file, int width = 800, int height = 600)
        {
            if (string.IsNullOrEmpty(file)) throw new ArgumentException("Invalid filename");
            if (width < 0) throw new ArgumentException("Invalid width");
            if (height < 0) throw new ArgumentException("Invalid height");
            Write("set terminal svg size {0}, {1}\n", width, height);
            Write("set output '{0}'\n", file);
            Replot();
            ResetTerminal();
        }

        public void Command(string cmd)
        {
            Write("{0}\n", cmd);
        }

        public void Dispose()
        {
            Stop();
            _gnuplot = null;
        }
    }
}
