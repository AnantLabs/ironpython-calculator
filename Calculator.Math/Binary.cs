using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginApi;

namespace Calculator.Mathematics
{
    [ReflectionLoad]
    [InputPanelGenerate(PanelName="Binary Funcions", UseAliasNames=true)]
    public static class Logics
    {
        private enum Operation
        {
            Not, Or, And, Eq, Xor
        }

        private static bool IsFloat(double num)
        {
            double n = num - Math.Floor(num);
            return n != 0;
        }

        private static double BinDouble(Operation o, double param1, double param2 = 0)
        {
            if (IsFloat(param1) || IsFloat(param2))
            {
                byte[] p1, p2, result;
                p1 = BitConverter.GetBytes(param1);
                p2 = BitConverter.GetBytes(param2);
                result = new byte[sizeof(double)];
                for (int i = 0; i < result.Length; i++)
                {
                    switch (o)
                    {
                        case Operation.Not:
                            result[i] = (byte)~p1[i];
                            break;
                        case Operation.And:
                            result[i] = (byte)(p1[i] & p2[i]);
                            break;
                        case Operation.Or:
                            result[i] = (byte)(p1[i] | p2[i]);
                            break;
                        case Operation.Xor:
                            result[i] = (byte)(p1[i] ^ p2[i]);
                            break;
                        case Operation.Eq:
                            result[i] = (byte)~(p1[i] ^ p2[i]);
                            break;
                    }
                }
                return BitConverter.ToDouble(result, 0);
            }
            else
            {
                long n1 = (long)param1;
                long n2 = (long)param2;
                switch (o)
                {
                    case Operation.Not:
                        return ~n1;
                    case Operation.And:
                        return n1 & n2;
                    case Operation.Or:
                        return n1 | n2;
                    case Operation.Xor:
                        return n1 ^ n2;
                    case Operation.Eq:
                        return ~(n1 ^ n2);
                    default:
                        return 0;
                }
            }
        }

        [Alias("Not")]
        public static double Not(double number)
        {
            return BinDouble(Operation.Not, number);
        }

        [Alias("Or")]
        public static double Or(double number1, double number2)
        {
            return BinDouble(Operation.Or, number1, number2);
        }

        [Alias("And")]
        public static double And(double number1, double number2)
        {
            return BinDouble(Operation.And, number1, number2);
        }

        [Alias("Eq")]
        public static double Eq(double number1, double number2)
        {
            return BinDouble(Operation.Eq, number1, number2);
        }

        [Alias("Xor")]
        public static double Xor(double number1, double number2)
        {
            return BinDouble(Operation.Xor, number1, number2);
        }

        [Alias("BitCount")]
        public static long Bitcount(long n)
        {
            if (IsFloat(n))
            {
                byte[] b1, r;
                long globaldist = 0;
                long dist = 0;
                long val;
                b1 = BitConverter.GetBytes(n);
                r = new byte[sizeof(double)];
                for (int i = 0; i < r.Length; i++)
                {
                    val = 0;
                    while (val != 0)
                    {
                        ++dist;
                        val &= val - 1;
                    }
                    globaldist += dist;
                }
                return globaldist;
            }
            else
            {
                long n1;
                n1 = (long)n;
                long dist = 0, val = 0;
                while (val != 0)
                {
                    ++dist;
                    val &= val - 1;
                }
                return dist;
            }
        }

        [Alias("Hamming")]
        public static double Hamming(double number1, double number2)
        {
            if (IsFloat(number1) || IsFloat(number2))
            {
                byte[] b1, b2, r;
                long globaldist = 0;
                long dist = 0;
                long val;
                b1 = BitConverter.GetBytes(number1);
                b2 = BitConverter.GetBytes(number2);
                r = new byte[sizeof(double)];
                for (int i = 0; i < r.Length; i++)
                {
                    val = b1[i] ^ b2[i];
                    while (val != 0)
                    {
                        ++dist;
                        val &= val - 1;
                    }
                    globaldist += dist;
                }
                return globaldist;
            }
            else
            {
                long n1, n2;
                n1 = (long)number1;
                n2 = (long)number2;
                long dist = 0, val = n1 ^ n2;
                while (val != 0)
                {
                    ++dist;
                    val &= val - 1;
                }
                return dist;
            }
        }

        [Alias("NumSysConv")]
        public static string NumSysConv(double input, int system)
        {
            StringBuilder sb = new StringBuilder();
            if (system != 2 && system != 8 && system != 10 & system != 16) throw new ArgumentException("System must be one of the following: 2,8,10,16");
            if (IsFloat(input))
            {
                byte[] bytes = BitConverter.GetBytes(input);
                foreach (var b in bytes) sb.Append(Convert.ToString(b, system));
                return sb.ToString().ToUpper();
            }
            else return Convert.ToString((long)input, system).ToUpper();
        }
    }
}
