using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using PluginApi;
using IronPython.Runtime;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calculator.Classes
{
    public static class Calculator
    {
        public static void Clear()
        {
            App.Core.ConsoleCont.Clear();
        }

        public static void SaveVariables(string target, params string[] names)
        {
            foreach (var n in names)
            {
                if (App.Core.VariablesNames.Contains(n)) continue;
                throw new ArgumentException(n + " is not defined");
            }
            VariableDumnp[] vars = new VariableDumnp[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                object o = App.Core.GetVariable(names[i]);
                if (!o.GetType().IsSerializable) throw new ArgumentException(names[i] + " can't be saved");
                vars[i] = new VariableDumnp { Name = names[i], Value = o };
            }
            if (File.Exists(target)) File.Delete(target);
            FileStream t = File.Create(target);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(t, vars);
            t.Close();
        }

        public static void LoadVariables(string source)
        {
            FileStream f = File.OpenRead(source);
            BinaryFormatter bf = new BinaryFormatter();
            VariableDumnp[] vars = (VariableDumnp[])bf.Deserialize(f);
            f.Close();
            foreach (var v in vars)
            {
                App.Core.SetVariable(v.Name, v.Value);
            }
        }

        public static void Chmode(string mode)
        {
            string m = mode.ToLower();
            switch (m)
            {
                case "deg":
                case "rad":
                case "grad":
                    App.Core.ChangeMode(m);
                    break;
                default:
                    throw new ArgumentException("Unknown mode");
            }
        }

        public static List NetList2PyList<T>(List<T> Source, bool ignoreorder = false)
        {
            List ret = new List();
            if (ignoreorder)
            {
                Parallel.ForEach(Source, item =>
                {
                    ret.Add(item);
                });
                return ret;
            }
            foreach (var item in Source) ret.Add(item);
            return ret;
        }

        public static List NetStack2List<T>(Stack<T> Source, bool ignoreorder = false)
        {
            List ret = new List();
            if (ignoreorder)
            {
                Parallel.ForEach(Source, item =>
                {
                    ret.Add(item);
                });
                return ret;
            }
            foreach (var item in Source) ret.Add(item);
            return ret;
        }

        public static List NetQueue2List<T>(Queue<T> Source, bool ignoreorder = false)
        {
            List ret = new List();
            if (ignoreorder)
            {
                Parallel.ForEach(Source, item =>
                {
                    ret.Add(item);
                });
                return ret;
            }
            foreach (var item in Source) ret.Add(item);
            return ret;
        }
    }
}
