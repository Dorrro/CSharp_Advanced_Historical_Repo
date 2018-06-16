namespace CSharp.Advanced.Sii.Trainings.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class IEnumerableExtensions{
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate, TSource defualtValue)
            where TSource : class 
        {
            return source.FirstOrDefault(predicate) ?? defualtValue;
        }

        public static IEnumerable<TSource> MakeSureToTake<TSource>(this IEnumerable<TSource> source, int count)
        {
            var counter = 0;

            foreach (var element in source)
            {
                yield return element;

                counter++;
                if(counter == count)
                    yield break;
            }

            throw new ArgumentException();
        }
    }
}