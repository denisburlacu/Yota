using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using Yota.Expressions;
using Yota.Guards;
using Yota.Helpers;
using Yota.Tests.Flags;

namespace Yota.Tests.Base
{
    public static class HHL
    {
        [Test]
        [TestCaseSource(nameof(LocationTestData))]
        public static void CheckFlag(TestEnum[] bitsToSet)
        {
            var handler = new TestHandler();
            foreach (var testEnum in bitsToSet)
            {
                YotaHelper.SetFlag(handler, testEnum);
            }

            var enumerable = Enumerable.Range(0, YotaGuard.GetMaxValue<ITestYota>())
                .Select(it => (TestEnum) it)
                .ToList();

            foreach (var @enum in enumerable)
            {
                var expression = ExpressionGenerator.HasFlagExpression<TestHandler, ITestYota, TestEnum>(@enum);
                var actual = expression.Compile()(handler);
                var expected = bitsToSet.Any(s => s == @enum);
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public static void Test()
        {
            CheckFlag(new []{(TestEnum.S3)});
        }
        
        private static IEnumerable<TestCaseData> LocationTestData()
        {
            var random = new Random((int) DateTime.UtcNow.Ticks);

            for (int i = 0; i < 1000; i++)
            {
                var totalBits = YotaGuard.GetMaxValue<ITestYota>();

                var bitsToSet = new List<TestEnum>();

                var totalSet = random.Next(1, 40);

                for (int j = 0; j < totalSet; j++)
                {
                    bitsToSet.Add((TestEnum) random.Next(0, totalBits));
                }

                var unOrdered = bitsToSet
                    .Distinct()
                    .OrderBy(x => random.Next()).ToArray();

                yield return new TestCaseData(unOrdered).SetName(string.Join(",", bitsToSet.Select(it => (int) it)));
            }
        }
        
        public static bool IsBitSet<T>(this T t, int pos) where T : struct, IConvertible
        {
            var value = t.ToInt64(CultureInfo.CurrentCulture);
            return (value & (1L << pos)) != 0;
        }
        
        public static long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }
    }
}