using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AvCore.Application.Interfaces;
using AvCore.Domain.Entities.policies;
using AvCore.Domain.Entities.scans;
using Microsoft.Extensions.Logging;


namespace AvCore.Application.Services
{
    public class FileScanner : IFileScanner
    {
        private readonly ILogger _logger;
        private readonly IHasher _hasher;
        private readonly ZipPolicy _policy;
        public FileScanner(ILogger logger, IHasher hasher,ZipPolicy policy)
        {
            _logger = logger;
            _hasher = hasher;
            _policy = policy;
        }
        public async Task ScanFileAsync(string path)
        {
            path = Path.GetFullPath(path);
            Stack<string> dirs = new Stack<string>();
            dirs.Push(path);


            // ENJOY THE SPAGHETTI :)
            while (dirs.Count > 0)
            {
                var currentDir = dirs.Pop();

                try
                {
                    var files = Directory.EnumerateFiles(currentDir);

                    foreach (var file in files) 
                    {
                        var ext = Path.GetExtension(file).ToLower();
                        
                        if(ext == ".zip")
                        {
                            await ProcessZipFileAsync(file);
                            break;
                        }
                        else
                        {
                            FileInfo fileInfo = new FileInfo(file);
                            await _hasher.HashFunc(file,fileInfo);
                            
                        }
                    
                    }
                    var directories = Directory.EnumerateDirectories(currentDir);

                    foreach (var dir in directories)
                    {
                        var enumFiles = Directory.EnumerateFiles(dir, "*");

                        foreach (var file in enumFiles)
                        {
                            var extension = Path.GetExtension(file).ToLower();

                            if (extension == ".zip")
                            {
                                await ProcessZipFileAsync(file);
                                break;
                            }
                            else
                            {
                                FileInfo fileInfo = new FileInfo(file);
                                await _hasher.HashFunc(file, fileInfo);
                            }

                        }

                        dirs.Push(dir);

                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task ProcessZipFileAsync(string file)
        {

        }

    }
}
