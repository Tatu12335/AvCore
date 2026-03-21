using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvCore.Domain.Entities.policies
{
    public class ZipPolicy
    {
        public long MaxTotalUncompressed { get; } = 100_000_000; // 100 MB
        public int MaxEntries { get; } = 1000;
        public double MaxCompressionRatio { get; } = 100.0; 
    }
}
