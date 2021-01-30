using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Yota.Guards;
using Yota.Helpers;
using Yota.Tests.Base;

namespace Yota.Tests.Flags
{
    public static class ExpressionTests
    {
        [Test]
        [TestCaseSource(nameof(LocationTestData))]
        public static void CheckBitsSetCorrectly(TestEnum[] bitsToSet)
        {
            var handler = new TestHandler();
            foreach (var testEnum in bitsToSet)
            {
                YotaHelper.SetFlag(handler, testEnum);
            }

            AssertBits(handler.Flag1, sizeof(byte) * 8,
                bitsToSet.Select(it => (int) it)
                    .ToArray());

            AssertBits(handler.Flag2, sizeof(int) * 8,
                bitsToSet.Select(it => (int) it)
                    .Select(it => it - (sizeof(byte) * 8))
                    .Where(it => it >= 0).ToArray());

            AssertBits(handler.Flag3, sizeof(long) * 8,
                bitsToSet.Select(it => (int) it)
                    .Select(it => it - (sizeof(byte) * 8) - (sizeof(int) * 8))
                    .Where(it => it >= 0).ToArray());
        }

        [Test]
        [TestCaseSource(nameof(LocationTestData))]
        public static void CheckFlagIsRemoved(TestEnum[] bitsToSet)
        {
            var handler = new TestHandler();
            foreach (var testEnum in bitsToSet)
            {
                YotaHelper.SetFlag(handler, testEnum);
            }

            foreach (var testEnum in bitsToSet)
            {
                YotaHelper.RemoveFlag(handler, testEnum);
            }

            var enumerable = Enumerable.Range(0, YotaGuard.GetMaxValue<ITestYota>())
                .Select(it => (TestEnum) it)
                .ToList();

            foreach (var @enum in enumerable)
            {
                var actual = YotaHelper.ContainsFlag(handler, @enum);
                Assert.AreEqual(false, actual);
            }
        }

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
                var actual = YotaHelper.ContainsFlag(handler, @enum);
                var expected = bitsToSet.Any(s => s == @enum);
                Assert.AreEqual(expected, actual);
            }
        }


        [Test]
        public static void Run()
        {
            var bitsToSet = new[] {98, 92, 8, 61, 90, 99, 96, 42, 53};
            CheckFlag(bitsToSet.Select(it => (TestEnum) it).ToArray());
        }

        private static void AssertBits(long value, int numberOfBits, params int[] settedBits)
        {
            var bits = ToBits(value, numberOfBits);
            for (var index = 0; index < bits.Length; index++)
            {
                var bit = bits[index];
                var expected = settedBits.Any(s => index == s);
                var actual = bit;
                if (expected != actual)
                {
                    Console.WriteLine();
                }

                Assert.AreEqual(expected, actual);
            }
        }

        private static bool[] ToBits(long input, int numberOfBits)
        {
            return Enumerable.Range(0, numberOfBits)
                .Select(bitIndex => (long) (1L << bitIndex))
                .Select(bitMask => (input & bitMask) != 0)
                // .Select(it => IsBitSet(input, it))
                .ToArray();
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
    }
}