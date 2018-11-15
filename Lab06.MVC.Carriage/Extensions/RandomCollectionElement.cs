using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Lab06.MVC.Carriage.Extensions
{
    public static class RandomCollectionElement
    {
        public static T TakeRandom<T>(this IEnumerable<T> source)
        {
            var array = source.ToArray();
            var helpRand = RandomNumberGenerator.Create();
            var bytes = new byte[array.Length-1];
            helpRand.GetBytes(bytes);
            var i = BitConverter.ToInt32(bytes, 0);
            var random = new Random(i);
            var index = random.Next(0, array.Length-1);

            return array[index];
        }
    }
}