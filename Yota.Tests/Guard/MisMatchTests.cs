using NUnit.Framework;
using Yota.Core;
using Yota.Exceptions;
using Yota.Guards;

namespace Yota.Tests.Guard
{
    public class MisMatchTests
    {
        [Test]
        public static void Ensure_Max_Length_Exception()
        {
            Assert.Throws<YotaEnumMismatchingException>(YotaGuard
                .EnsureContainsEnoughValues<IInvalidYota, InvalidYotaEnum>);
        }

        private interface IInvalidYota : IBaseYota
        {
            public byte Yota { get; set; }
        }

        public enum InvalidYotaEnum
        {
            Test = int.MaxValue
        }

        private class InvalidMaxLength : IHandler<IInvalidYota, InvalidYotaEnum>, IInvalidYota
        {
            public byte Yota { get; set; }
        }
    }
}