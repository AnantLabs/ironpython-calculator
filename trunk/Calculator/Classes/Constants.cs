using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator.Classes
{
    internal class ConstantManager
    {
        private Dictionary<string, double> _consts;
        private Dictionary<string, double> _prefixes;

        public ConstantManager()
        {
            _consts = new Dictionary<string, double>();
            _prefixes = new Dictionary<string, double>();

            _prefixes.Add("Yotta", 1e24);
            _prefixes.Add("Zetta", 1e21);
            _prefixes.Add("Exa", 1e18);
            _prefixes.Add("Peta", 1e15);
            _prefixes.Add("Tera", 1e12);
            _prefixes.Add("Giga", 1e9);
            _prefixes.Add("Mega", 1e6);
            _prefixes.Add("Kilo", 1e3);
            _prefixes.Add("Hecto", 1e2);
            _prefixes.Add("Deca", 1e1);
            _prefixes.Add("Deci", 1e-1);
            _prefixes.Add("Centi", 1e-2);
            _prefixes.Add("Milli", 1e-3);
            _prefixes.Add("Micro", 1e-6);
            _prefixes.Add("Nano", 1e-9);
            _prefixes.Add("Pico", 1e-12);
            _prefixes.Add("Femto", 1e-15);
            _prefixes.Add("Atto", 1e-18);
            _prefixes.Add("Zepto", 1e-21);
            _prefixes.Add("Yocto", 1e-24);

            _consts.Add("Pi", Math.PI);
            _consts.Add("E", Math.E);
            _consts.Add("Euler-Mascheroni", 0.5772156649015328606065120900824024310421593359399235988057672348849d);
            _consts.Add("Golden Ratio", 1.6180339887498948482045868343656381177203091798057628621354486227052604628189024497072d);
            _consts.Add("Speed Of Light", 2.99792458e8);
            _consts.Add("Magnetic Permeability", 1.2566370614359172953850573533118011536788677597500e-6);
            _consts.Add("Electric Permittivity", 8.8541878171937079244693661186959426889222899381429e-12);
            _consts.Add("Gravitational Constant", 6.67429e-11);
            _consts.Add("Plancks Constant", 6.62606896e-34);

        }

        public double this[string key]
        {
            get 
            {
                if (_prefixes.ContainsKey(key)) return _prefixes[key];
                else return _consts[key]; 
            }
        }

        public string[] Constants
        {
            get { return (from i in _consts.Keys orderby i ascending select i).ToArray(); }
        }

        public string[] Prefixes
        {
            get { return (from i in _prefixes.Keys orderby i ascending select i).ToArray(); }
        }
    }
}
