using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginApi;

namespace Calculator.Mathematics
{
    [ReflectionLoad]
    [InputPanelGenerate(PanelName="Special Functions", UseAliasNames=true)]
    public static class Specials
    {
        private static int[] Primes = new int[]
        {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239,
            293, 353, 431, 521, 631, 761, 919, 1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861,
            5839, 7013, 8419, 10103, 12143, 14591, 17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523,
            108631, 130363, 156437, 187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263, 1674319,
            2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369
        };

        [Alias("Ack")]
        public static long Ackermann(long m, long n)
        {
            if (m > 0)
            {
                if (n > 0) return Ackermann(m - 1, Ackermann(m, n - 1));
                else if (n == 0) return Ackermann(m - 1, 1);
            }
            else if (m == 0)
            {
                if (n >= 0) return n + 1;
            }
            throw new System.ArgumentOutOfRangeException();
        }

        [Alias("IsTriangle")]
        public static double IsTriangle(double a, double b, double c)
        {
            bool test = (a + b > c) && (a + c > b) && (b + c > a);
            if (test) return 1;
            else return 0;
        }

        [Alias("IsPrime")]
        public static double IsPrime(double n)
        {
            double num = Math.Abs(Math.Truncate(n));

            if (Primes.Contains((int)num)) return 1;
            if ((int)num < Primes.Max()) return 0;
            if (n % 2 == 0 && n != 2) return 0;

            double i;

            for (i = Primes[Primes.Length - 1]; (i * i) <= n; i += 2)
            {
                if (n % i == 0) return 0;
            }
            return 1;
        }

        [Alias("Fibonacci")]
        public static double Fibonacci(double limit)
        {
            double sqrt5 = Math.Sqrt(5);
            double tmp = Math.Pow((1 + sqrt5), limit) - Math.Pow((1 - sqrt5), limit);
            return tmp / (Math.Pow(2, limit) * sqrt5);
        }
    }
}
