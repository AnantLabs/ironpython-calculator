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
        public Alias(string aliasname)
        {
            if (string.IsNullOrEmpty(aliasname)) throw new ArgumentException("aliasname can't be null");
            this.AliasName = aliasname;
        }
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
}
