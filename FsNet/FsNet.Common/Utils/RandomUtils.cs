using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace FsNet.Common.Utils
{
    public class RandomUtils
    {
        private static readonly ThreadLocal<Random> RndLocal = new ThreadLocal<Random>(() => new Random(GetUniqueSeed()));

        private static int GetUniqueSeed()
        {
            long next, current;
            var guid = Guid.NewGuid().ToByteArray();
            var seed = BitConverter.ToInt64(guid, 0);

            do
            {
                current = Interlocked.Read(ref seed);
                next = current * BitConverter.ToInt64(guid, 3);
            } while (Interlocked.CompareExchange(ref seed, next, current) != current);

            return (int)next ^ Environment.TickCount;
        }

        public static int GetRandom(int min, int max)
        {
            Contract.Assert(max >= min);
            return RndLocal.Value.Next(min, max);
        }

        public static int GetRandom(int max)
        {
            return RndLocal.Value.Next(max);
        }

        public static double GetRandom()
        {
            return RndLocal.Value.NextDouble();
        }
    }
}