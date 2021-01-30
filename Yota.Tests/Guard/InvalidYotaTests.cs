using NUnit.Framework;
using Yota.Core;
using Yota.Exceptions;
using Yota.Guards;

namespace Yota.Tests.Guard
{
    public class InvalidYotaTests
    {
        [Test]
        public static void Ensure_Exception_When_Invalid_Yota()
        {
            Assert.Throws<YotaEnumMismatchingException>(YotaGuard
                .EnsureContainsEnoughValues<IInvalidYota, InvalidYotaEnum>);
        }

        private interface IInvalidYota : IBaseYota
        {
        }

        public enum InvalidYotaEnum
        {
            Test = int.MaxValue
        }

        private class InvalidMaxLength : IYotaEntity<IInvalidYota, InvalidYotaEnum>, IInvalidYota
        {
            public byte Yota { get; set; }
        }
    }
}