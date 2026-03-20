using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvCore.Application.Interfaces
{
    public interface IHasher
    {
        public Task<string> HashFunc(string file,FileInfo fileInfo);
    }
}
