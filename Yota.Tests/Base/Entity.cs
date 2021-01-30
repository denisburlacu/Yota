using System.ComponentModel.DataAnnotations;
using Yota.Core;
using Yota.Tests.Flags;

namespace Yota.Tests.Base
{
    public class Entity : IHandler<ITestYota, TestEnum>
    {
        [Key]
        public int Id { get; set; }
        public long Flag1 { get; set; }
        public long Flag2 { get; set; }
        public long Flag3 { get; set; }
    }
}