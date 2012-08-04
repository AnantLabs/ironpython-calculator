using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginApi;

namespace Calculator.Mathematics
{
    [ReflectionLoad]
    [InputPanelGenerate(UseAliasNames=true, PanelName="Trigonometry")]
    public static class Trigonometry
    {
        public static CalculatorMode CalcMode { get; set; }

        static Trigonometry()
        {
            CalcMode = CalculatorMode.Deg;
        }

        [Alias("Rad2Deg")]
        public static double Rad2Deg(double rad)
        {
            return (rad * 180) / Math.PI;
        }

        [Alias("Deg2Rad")]
        public static double Deg2Rad(double deg)
        {
            return (Math.PI / 180) * deg;
        }

        [Alias("Deg2Grad")]
        public static double Deg2Grad(double deg)
        {
            return (400.0 / 360.0) * deg;
        }

        [Alias("Grad2Deg")]
        public static double Grad2Deg(double grad)
        {
            return (360.0 / 400.0) * grad;
        }

        [Alias("Grad2Rad")]
        public static double Grad2Rad(double grad)
        {
            double fok = (360.0 / 400.0) * grad;
            return (Math.PI / 180) * fok;
        }

        [Alias("Rad2Grad")]
        public static double Rad2Grad(double rad)
        {
            double fok = (rad * 180) / Math.PI;
            return (400.0 / 360.0) * fok;
        }

        [Alias("Sin")]
        public static double Sinus(double value1)
        {
            switch (CalcMode)
            {
                case CalculatorMode.Deg:
                    if ((Deg2Rad(value1) >= Math.PI) && ((Deg2Rad(value1) % Math.PI) == 0)) return 0;
                    else return Math.Sin(Deg2Rad(value1));
                case CalculatorMode.Grad:
                    if ((Grad2Rad(value1) >= Math.PI) && ((Grad2Rad(value1) % Math.PI) == 0)) return 0;
                    else return Math.Sin(Grad2Rad(value1));
                case CalculatorMode.Rad:
                    if ((value1 >= Math.PI) && ((value1 % Math.PI) == 0)) return 0;
                    else return Math.Sin(value1);
                default:
                    return double.NaN;
            }
        }

        [Alias("Cos")]
        public static double Cosinus(double value1)
        {
            switch (CalcMode)
            {
                case CalculatorMode.Deg:
                    if ((((Deg2Rad(value1) - (Math.PI / 2)) % Math.PI) == 0) || Deg2Rad(value1) == (Math.PI / 2)) return 0;
                    else return Math.Cos(Deg2Rad(value1));
                case CalculatorMode.Grad:
                    if ((((Grad2Rad(value1) - (Math.PI / 2)) % Math.PI) == 0) || Grad2Rad(value1) == (Math.PI / 2)) return 0;
                    else return Math.Cos(Grad2Rad(value1));
                case CalculatorMode.Rad:
                    if ((((value1 - (Math.PI / 2)) % Math.PI) == 0) || value1 == (Math.PI / 2)) return 0;
                    else return Math.Cos(value1);
                default:
                    return double.NaN;
            }
        }

        [Alias("Tan")]
        public static double Tangent(double value1)
        {
            return Trigonometry.Sinus(value1) / Trigonometry.Cosinus(value1);
        }

        [Alias("Sinh")]
        public static double SinHype(double value1)
        {
            switch (CalcMode)
            {
                case CalculatorMode.Deg:
                    return Math.Sinh(Deg2Rad(value1));
                case CalculatorMode.Grad:
                    return Math.Sinh(Grad2Rad(value1));
                case CalculatorMode.Rad:
                    return Math.Sinh(value1);
                default:
                    return double.NaN;
            }
        }

        [Alias("Cosh")]
        public static double CosHype(double value1)
        {
            switch (CalcMode)
            {
                case CalculatorMode.Deg:
                    return Math.Cosh(Deg2Rad(value1));
                case CalculatorMode.Rad:
                    return Math.Cosh(Grad2Rad(value1));
                case CalculatorMode.Grad:
                    return Math.Cosh(value1);
                default:
                    return double.NaN;
            }
        }

        [Alias("ArcSin")]
        public static double ArcusSinus(double value1)
        {
            switch (CalcMode)
            {
                case CalculatorMode.Deg:
                    return Rad2Deg(Math.Asin(value1));
                case CalculatorMode.Grad:
                    return Rad2Grad(Math.Asin(value1));
                case CalculatorMode.Rad:
                    return Math.Asin(value1);
                default:
                    return double.NaN;
            }
        }

        [Alias("ArcCos")]
        public static double ArcusCosinus(double value1)
        {
            switch (CalcMode)
            {
                case CalculatorMode.Deg:
                    return Rad2Deg(Math.Acos(value1));
                case CalculatorMode.Grad:
                    return Rad2Grad(Math.Acos(value1));
                case CalculatorMode.Rad:
                    return Math.Acos(value1);
                default:
                    return double.NaN;
            }
        }

        [Alias("ArcTan")]
        public static double ArcusTangent(double value1)
        {
            switch (CalcMode)
            {
                case CalculatorMode.Deg:
                    return Rad2Deg(Math.Atan(value1));
                case CalculatorMode.Grad:
                    return Rad2Grad(Math.Atan(value1));
                case CalculatorMode.Rad:
                    return Math.Atan(value1);
                default:
                    return double.NaN;
            }
        }

        [Alias("ArcSinh")]
        public static double ArcusSinHype(double value1)
        {
            double inrads = Math.Log(Math.Pow(Math.Pow(value1, 2) + 1, 0.5), Math.E);
            switch (CalcMode)
            {
                case CalculatorMode.Deg:
                    return Rad2Deg(inrads);
                case CalculatorMode.Grad:
                    return Rad2Grad(inrads);
                case CalculatorMode.Rad:
                    return inrads;
                default:
                    return double.NaN;
            }
        }

        [Alias("ArcCosh")]
        public static double ArcusCosHype(double value1)
        {
            double inrads = Math.Log(Math.Pow(Math.Pow(value1, 2) - 1, 0.5), Math.E);
            switch (CalcMode)
            {
                case CalculatorMode.Deg:
                    return Rad2Deg(inrads);
                case CalculatorMode.Grad:
                    return Rad2Grad(inrads);
                case CalculatorMode.Rad:
                    return inrads;
                default:
                    return double.NaN;
            }
        }

        [Alias("ArcTanh")]
        public static double ArcusTanHype(double value1)
        {
            double inrads = 0.5 * Math.Log((1 + value1 / 1 - value1), Math.E);
            switch (CalcMode)
            {
                case CalculatorMode.Deg:
                    return Rad2Deg(inrads);
                case CalculatorMode.Grad:
                    return Rad2Grad(inrads);
                case CalculatorMode.Rad:
                    return inrads;
                default:
                    return double.NaN;
            }
        }
    }
}
