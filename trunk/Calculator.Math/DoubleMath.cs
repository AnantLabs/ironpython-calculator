using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginApi;
using System.Security.Cryptography;

namespace Calculator.Mathematics
{
    [ReflectionLoad]
    public static class DoubleMath
    {
        [Alias("Mod")]
        public static double Modulo(double num1, double num2)
        {
            return num1 % num2;
        }

        [Alias("Inv")]
        public static double Inverse(double X)
        {
            return 1 / X;
        }

        [Alias("Replus")]
        public static double Replus(double num1, double num2)
        {
            return (num1 * num2) / (num1 + num2);
        }

        [Alias("Sqr")]
        public static double Sqr(double value1, double value2)
        {
            return Math.Pow(value1, (1 / value2));
        }

        [Alias("Pow")]
        public static double Pow(double val1, double val2)
        {
            return Math.Pow(val1, val2);
        }

        [Alias("Log")]
        public static double Log(double value1, double basen)
        {
            return Math.Log(value1, basen);
        }

        [Alias("Percent")]
        public static double Percent(double val1, double val2)
        {
            return (val2 / val1) * 100;
        }

        [Alias("Fact")]
        public static double Factor(double limit)
        {
            double result = 1;
            for (int i = 2; i <= limit; i++)
            {
                result *= i;
            }
            return result;
        }

        [Alias("PercentOf")]
        public static double PercentOf(double x, double percent)
        {
            double perc = x / 100;
            return perc * percent;
        }

        [Alias("Gcd")]
        public static double Gcd(double x, double y) //LNKO
        {
            if ((x == 0) || (y == 0)) throw new ArgumentException("Can't divide with zero!");
            while (x != y)
            {
                if (x > y) x = x - y;
                else y = y - x;
            }
            return x;
        }

        [Alias("Lcm")]
        public static double Lcm(double x, double y)
        {
            return Math.Round((x * y) / Gcd(x, y), 0);
        }

        [Alias("Db10")]
        public static double Db10(double x0, double x)
        {
            return 10 * Math.Log10(x / x0);
        }

        [Alias("Db20")]
        public static double Db20(double x0, double x)
        {
            return 20 * Math.Log10(x / x0);
        }

        [Alias("Rand")]
        public static double Random(int min, int max)
        {
            System.Random r = new Random();
            return r.Next(min, max);
        }

        [Alias("Randf")]
        public static double RandomFloat()
        {
            System.Random r = new Random();
            return r.NextDouble();
        }

        [Alias("RandBit")]
        public static double RandBitgen(int bits)
        {
            if (bits < 0 || bits > 64) throw new ArgumentException("Bits must be in range of 0 and 64");
            RNGCryptoServiceProvider prov = new RNGCryptoServiceProvider();
            byte[] tmp = new byte[8];
            prov.GetBytes(tmp);
            ulong number = BitConverter.ToUInt64(tmp, 0);
            ulong mask = (0xFFFFFFFFFFFFFFFF) >> bits;
            return number & mask;
        }
    }
}
