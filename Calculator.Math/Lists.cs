using System;
using System.Threading.Tasks;
using IronPython.Runtime;
using PluginApi;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Calculator.Mathematics
{
    [ReflectionLoad]
    [InputPanelGenerate(PanelName="List Functions", UseAliasNames=false)]
    public static class Lists
    {
        [Alias(InputText="Mege lists")]
        public static List Merge(List l1, List l2)
        {
            List ret = new List();
            Parallel.ForEach(l1, item =>
                {
                    ret.Add(item);
                });
            Parallel.ForEach(l2, item =>
                {
                    ret.Add(item);
                });
            return ret;
        }

        [Alias(InputText = "Clone List")]
        public static List Clone(List l)
        {
            List ret = new List();
            Parallel.ForEach(l, item =>
                {
                    ret.Add(item);
                });
            return ret;
        }

        [Alias(InputText = "Subtract Lists")]
        public static List Subtract(List l1, List l2)
        {
            List ret = Clone(l1);
            Parallel.ForEach(l2, item =>
                {
                    if (ret.Contains(item)) ret.remove(item);
                });
            return ret;
        }

        [Alias(InputText = "Excerpt of lists")]
        public static List Excerpt(List l1, List l2)
        {
            List ret = new List();
            Parallel.For(0, Math.Min(l1.Count, l2.Count), i =>
            {
                if (l1.Contains(l2[i])) ret.Add(l1[i]);
            });
            return ret;
        }

        [Alias(InputText = "Randomize Item order")]
        public static void RandOrder(List l)
        {
            List<object> ret = l.OrderBy(emp => Guid.NewGuid()).AsParallel().ToList();
            l.Clear();
            Parallel.ForEach(ret, item =>
            {
                l.Add(item);
            });
        }

        [Alias(InputText = "Fill with random numbers")]
        public static void RandFill(List l, int min, int max, int count)
        {
            Random r = new Random();
            l.Clear();
            Parallel.For(0, count, i =>
                {
                    l.Add(r.Next(min, max));
                });
        }

        [Alias(InputText = "Fill with random floats")]
        public static void RandFillf(List l, int count)
        {
            Random r = new Random();
            l.Clear();
            Parallel.For(0, count, i =>
            {
                l.Add(r.NextDouble());
            });
        }

        [Alias(InputText = "Fill with value")]
        public static void Fill(List l, int count, double value)
        {
            l.Clear();
            Parallel.For(0, count, i =>
                {
                    l.Add(value);
                });
        }
    }
}
