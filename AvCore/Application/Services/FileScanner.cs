using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvCore.Application.Interfaces;
using Microsoft.Extensions.Logging;


namespace AvCore.Application.Services
{
    public class FileScanner : IFileScanner
    {
        private readonly ILogger _logger;
        public FileScanner(ILogger logger) 
        { 
            _logger = logger;
        }


    }
}
