using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using Robocode;

namespace AndrewTatham.Helpers
{
    public static class ExtensionMethods
    {
        public static double StdDev(this IEnumerable<double> values)
        {
            double ret = 0;
            int count = values.Count();
            if (count > 1)
            {
                //Compute the Average
                double avg = values.Average();

                //Perform the Sum of (value-avg)^2
                double sum = values.Sum(d => (d - avg) * (d - avg));

                //Put it all together
                ret = Math.Sqrt(sum / count);
            }
            return ret;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        public static TValue SafeGet<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
            where TValue : class
        {
            return dic.ContainsKey(key) ? dic[key] : null;
        }

        public static IEnumerable<IEnumerable<T>> SplitIntoNGroups<T>(this IEnumerable<T> values, int n)
        {
            return values
                .Select((item, i) => new { i, item })
                .GroupBy(k => k.i % n, v => v.item)
                .Select(g => g.AsEnumerable());
        }

        public static IEnumerable<IEnumerable<T>> SplitIntoGroupsOf<T>(this IEnumerable<T> values, int n)
        {
            return values
                .Select((item, i) => new { i, item })
                .GroupBy(k => k.i / n, v => v.item)
                .Select(g => g.AsEnumerable());
        }

        public static Vector Mode(this IEnumerable<Vector> values, int groupBy)
        {
            if (values != null && values.Any())
            {
                return values.GroupBy(k => new
                {
                    x = groupBy * (int)k.X / groupBy,
                    y = groupBy * (int)k.Y / groupBy
                })
                    .OrderByDescending(g => g.Count())
                    .First()
                    .First();
            }
            return null;
        }

        public static object[,] To2DArray(this IEnumerable<IEnumerable<object>> values,
            IEnumerable<string> columns)
        {
            int x = values.Count();
            int y = columns.Count();
            var retval = new object[x + 1, y];

            // column headings
            var columnHeadings = columns.ToArray();
            for (int k = 0; k < y; k++)
            {
                retval[0, k] = columnHeadings[k];
            }
            int i = 1;
            values.ForEach(row =>
            {
                int j = 0;
                var rowArray = row.ToArray();
                rowArray.ForEach(val =>
                {
                    retval[i, j] = val;
                    j++;
                });
                i++;
            });

            return retval;
        }

        public static IEnumerable<T> EveryOneIn<T>(this IEnumerable<T> values, int mod)
        {
            return values.TakeWhile((v, i) => i % mod == 0);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> values)
        {
            return values.OrderBy(x => RandomHelper.Next());
        }

        public static string ToCsv<T>(this IEnumerable<T> values)
        {
            return values.Select(v => v.ToString()).Aggregate((v1, v2) => v1 + ", " + v2);
        }

        public static List<T> Last<T>(this List<T> values, int count)
        {
            return values.Count > count ? values.GetRange(values.Count - count, count) : values;
        }

        public static List<T> First<T>(this List<T> values, int count)
        {
            return values.Count > count ? values.GetRange(0, count) : values;
        }

        public static void Render(this Vector vector, IGraphics graphics, Color colour)
        {
            Contract.Requires(vector != null);
            Contract.Requires(graphics != null);
            Contract.Requires(colour != null);
            if (vector != null)
            {
                using (var newPen = new Pen(colour))
                {
                    graphics.DrawRectangle(newPen, -2 + (int)vector.X, -2 + (int)vector.Y, 5, 5);
                }
            }
        }

        public static void Render(this IEnumerable<Vector> vectors, IGraphics graphics, Pen pen)
        {
            Contract.Requires(vectors != null);
            Contract.Requires(graphics != null);
            Contract.Requires(pen != null);
            if (vectors != null && graphics != null && vectors != null && vectors.Count() > 1)
            {
                graphics.DrawPolygon(pen,
                                     vectors.Select(vector => new PointF((float)vector.X, (float)vector.Y)).ToArray());
            }
        }

        [Obsolete]
        public static void Render(this IEnumerable<Vector> vectors, IGraphics graphics, Color colour)
        {
            Contract.Requires(vectors != null);
            Contract.Requires(graphics != null);
            Contract.Requires(colour != null);
            using (var newPen = new Pen(colour))
            {
                graphics.DrawPolygon(newPen,
                                     vectors.Select(vector => new PointF((float)vector.X, (float)vector.Y)).ToArray());
            }
        }

        public static Dictionary<TKey, List<TValue>> ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> groupings)
        {
            return groupings.ToDictionary(group => group.Key, group => group.ToList());
        }

        public static Dictionary<TFirstKey,
            Dictionary<TSecondKey, TValue>>
            Pivot<TSource, TFirstKey, TSecondKey, TValue>(
            this IEnumerable<TSource> source,
            Func<TSource, TFirstKey> firstKeySelector,
            Func<TSource, TSecondKey> secondKeySelector,
            Func<IEnumerable<TSource>, TValue> aggregate)
        {
            var retVal = new Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>();

            var l = source.ToLookup(firstKeySelector);
            foreach (var item in l)
            {
                var dict = new Dictionary<TSecondKey, TValue>();
                retVal.Add(item.Key, dict);
                var subdict = item.ToLookup(secondKeySelector);
                foreach (var subitem in subdict)
                {
                    dict.Add(subitem.Key, aggregate(subitem));
                }
            }

            return retVal;
        }

        public static object[]
    PivotChart<TSource, TFirstKey, TSecondKey, TValue>(
    this IEnumerable<TSource> source,
    Func<object> firstCellSelector,
    Func<TSource, TFirstKey> firstKeySelector,
    Func<TFirstKey, object> firstKeyLabelSelector,
    Func<TSource, TSecondKey> secondKeySelector,
    Func<TSecondKey, object> secondKeyLabelSelector,
    Func<IEnumerable<TSource>, TValue> aggregate,
    Func<object> noValue
            )
        {
            var dic = source.Pivot(
                firstKeySelector,
                secondKeySelector,
                aggregate);

            var allColKeys = dic
                .SelectMany(kvp0 => kvp0.Value.Keys)
                .Distinct()
                .OrderBy(x => x);

            var rows = new object[dic.Keys.Count + 1];
            rows[0] = new[]
            {
                firstCellSelector()
            }
            .Concat(allColKeys.Select(secondKeyLabelSelector))
                .ToArray();
            int rowno = 1;
            foreach (var kvp0 in dic)
            {
                var cols = new object[allColKeys.Count() + 1];
                foreach (var kvp1 in kvp0.Value)
                {
                    rows[rowno] = new[]
                    {
                        firstKeyLabelSelector(kvp0.Key)
                    }
                    .Concat(allColKeys.Select(colKey => kvp0.Value.ContainsKey(colKey) ? kvp0.Value[colKey] : noValue()))
                    .ToArray();
                }
                rowno++;
            }

            return rows;
        }
    }
}