using Yota.Core;
using Yota.Tests.Flags;

namespace Yota.Tests.Base
{
    public class TestYotaEntity : IYotaEntity<ITestYota, TestEnum>, ITestYota
    {
        public byte Flag1 { get; set; }
        public int Flag2 { get; set; }
        public long Flag3 { get; set; }
    }
}