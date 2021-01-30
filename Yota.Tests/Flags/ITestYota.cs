using Yota.Core;

namespace Yota.Tests.Flags
{
    public interface ITestYota : IBaseYota
    {
        byte Flag1 { get; set; }
        int Flag2 { get; set; }
        long Flag3 { get; set; }
    }
}