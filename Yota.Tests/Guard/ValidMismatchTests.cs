using NUnit.Framework;
using Yota.Core;
using Yota.Guards;

namespace Yota.Tests.Guard
{
    public class ValidMismatchTests
    {
        [Test]
        public static void Ensure_Valid_Yota_Configuration()
        {
            Assert.DoesNotThrow(YotaGuard
                .EnsureContainsEnoughValues<IValidYota, YotaEnum>);
        }

        private interface IValidYota : IBaseYota
        {
            public byte Yota { get; set; }
        }

        public enum YotaEnum
        {
            Test = 8
        }
    }
}