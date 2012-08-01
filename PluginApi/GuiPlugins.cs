using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace PluginApi
{
    public abstract class GuiPlugin : UserControl
    {
        /// <summary>
        /// Plugin host, API can be accesed through it
        /// </summary>
        public PluginHost Host
        {
            get;
            set;
        }

        /// <summary>
        /// MenuItem name that will be displayed
        /// </summary>
        public string MenuItemName { get; set; }

        /// <summary>
        /// MenuItem tooltip
        /// </summary>
        public string MenuItemTooltip { get; set; }
    }

    public interface PluginHost
    {
        /// <summary>
        /// Registers a Plugin
        /// </summary>
        /// <param name="plugin">Plugin to register</param>
        void Register(GuiPlugin plugin);

        /// <summary>
        /// Runs a Python command string
        /// </summary>
        /// <param name="s">Python command string</param>
        /// <returns>true, if execution was succesfull, false if exception occured</returns>
        bool RunPythonCommand(string s);

        /// <summary>
        /// Gets a List of defined variables
        /// </summary>
        string[] Variables { get; }

        /// <summary>
        /// Gets variable data
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <returns></returns>
        object GetVariable(string name);

        /// <summary>
        /// Sets a variable value
        /// </summary>
        /// <param name="name">variable name</param>
        /// <param name="value">Value to asign to it</param>
        void SetVariable(string name, object value);
    }
}
