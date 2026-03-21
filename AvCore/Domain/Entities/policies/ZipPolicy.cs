using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvCore.Domain.Entities.policies
{
    public class ZipPolicy
    {
        public const long MaxTotalUncompressed = 100_000_000; // 100 MB
        public const int MaxEntries = 1000;
        public const double MaxCompressionRatio = 100.0;
    }
}
