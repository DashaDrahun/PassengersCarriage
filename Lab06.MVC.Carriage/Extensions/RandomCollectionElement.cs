using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab06.MVC.Carriage.Extensions
{
    public static class RandomCollectionElement
    {
        public static T TakeRandom<T>(this IEnumerable<T> source)
        {
            var array = source.ToArray();
            var random = new Random();
            var index = random.Next(0, array.Length-1);

            return array[index];
        }
    }
}