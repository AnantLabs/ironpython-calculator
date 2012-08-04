using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginApi
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Alias: Attribute
    {
        public string AliasName { get; set; }
        public string InputText { get; set; }
        public Alias(string aliasname)
        {
            this.AliasName = aliasname;
        }

        public Alias() {}
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class ReflectionLoad: Attribute
    {
    }

    public enum CalculatorMode
    {
        Deg = 1, Rad = 2, Grad = 3
    }

    [Serializable]
    public class VariableDumnp
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Value.GetHashCode();
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class InputPanelGenerate: Attribute
    {
        public string PanelName { get; set; }
        public bool UseAliasNames { get; set; }
    }
}
