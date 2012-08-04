using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginApi;
using System.Threading.Tasks;

namespace Calculator.Mathematics
{
    [ReflectionLoad]
    [InputPanelGenerate(PanelName="Statistics", UseAliasNames=false)]
    public static class Stat
    {
        [Alias(InputText="Minimum")]
        public static double Min(IronPython.Runtime.List list)
        {
            return list.AsParallel().Min(i => Convert.ToDouble(i));
        }

        [Alias(InputText = "Maximum")]
        public static double Max(IronPython.Runtime.List list)
        {
            return list.AsParallel().Max(i => Convert.ToDouble(i));
        }

        [Alias(InputText = "Sum")]
        public static double Sum(IronPython.Runtime.List list)
        {
            return list.AsParallel().Sum(i => Convert.ToDouble(i));
        }

        [Alias(InputText = "Count")]
        public static double Count(IronPython.Runtime.List list)
        {
            return list.Count;
        }

        [Alias(InputText = "Average")]
        public static double Avg(IronPython.Runtime.List list)
        {
            return list.AsParallel().Average(i => Convert.ToDouble(i));
        }

        [Alias(InputText = "Harmonic Average")]
        public static double AvgHarmonic(IronPython.Runtime.List list)
        {
            double ret = 0;
            Parallel.ForEach(list.Cast<double>(), item =>
                {
                    ret +=  (1 / item);
                });
            return list.Count / ret;
        }

        [Alias(InputText = "Geometric Average")]
        public static double AvgGeometric(IronPython.Runtime.List list)
        {
            double ret = 1;
            Parallel.ForEach(list.Cast<double>(), item =>
            {
                ret *= item;
            });
            return Math.Pow(ret, 1 / list.Count);
        }

        [Alias(InputText = "Square Average")]
        public static double AvgSquare(IronPython.Runtime.List list)
        {
            double ret = 0;
            Parallel.ForEach(list.Cast<double>(), item =>
            {
                ret += Math.Pow(item, 2);
            });
            return Math.Pow(ret / list.Count, 1 / 2);
        }

        [Alias(InputText = "Deviation")]
        public static double Deviat(IronPython.Runtime.List list)
        {
            double ret = 0;
            double atlag = Avg(list);
            Parallel.ForEach(list, item =>
            {
                ret += Math.Pow(Convert.ToDouble(item) - atlag, 2);
            });
            return ret / list.Count;
        }

        [Alias(InputText = "Standard Deviation")]
        public static double DeviatStandard(IronPython.Runtime.List list)
        {
            return Math.Sqrt(Variance(list));
        }

        [Alias(InputText = "Range")]
        public static double Range(IronPython.Runtime.List list)
        {
            return Max(list) - Min(list);
        }

        [Alias(InputText = "Median")]
        public static double Median(IronPython.Runtime.List list)
        {
            var q = (from i in list orderby Convert.ToDouble(i) ascending select Convert.ToDouble(i)).AsParallel().ToList();
            int index = q.Count / 2 - 1;
            if (q.Count % 2 == 0) return (q[index] + q[index - 1]) / 2.0;
            return q[index];
        }

        [Alias(InputText = "Maximum")]
        public static double Variance(IronPython.Runtime.List list)
        {
            double avg = Avg(list);
            double d = list.AsParallel().Aggregate(0.0, (total, next) => total += Math.Pow(Convert.ToDouble(next) - Convert.ToDouble(avg), 2)); 
            return d / (list.Count - 1);
        }

        [Alias(InputText = "Mode")]
        public static double Mode(IronPython.Runtime.List list)
        {
            var sortedList = (from number in list orderby Convert.ToDouble(number) select Convert.ToDouble(number)).AsParallel().ToList();
            int count = 0;
            int max = 0;
            double current = sortedList[0];
            double mode = 0;
            foreach (var i in sortedList)
            {
                if (current != i)
                {
                    current = i;
                    count = 1;
                }
                else count++;

                if (count > max)
                {
                    max = count;
                    mode = current;
                }

                if (max > 1) return mode;
            }
            return mode;
        }

        [Alias(InputText = "Covariance")]
        public static double Covariance(IronPython.Runtime.List source, IronPython.Runtime.List other)
        {
            int len = source.Count;

            double avgSource = Avg(source);
            double avgOther = Avg(other);
            double covariance = 0;


            Parallel.For(0, len, i => 
                {
                    covariance += (Convert.ToDouble(source[i]) - avgSource) * (Convert.ToDouble(other[i]) - avgOther);
                });

            return covariance / len;
        }
    }
}
