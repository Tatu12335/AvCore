using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvCore.Application.Interfaces
{
    public interface IOpenRead
    {
        public Task<FileStream> OpenAsync(string archive);
    }
}
