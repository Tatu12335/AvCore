
using AvCore.Application.Interfaces;
using AvCore.Domain.Entities.policies;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AvCore.Application.Services
{
    public class FileScanner : IFileScanner
    {
        private readonly IHasher _hasher;
        private readonly ZipPolicy _policy;
        private readonly IZipArcvhiveService _zipArcvhiveService;
        private readonly IOpenRead _openRead;
        private readonly ILogger<FileScanner> _logger;

        public FileScanner(IHasher hasher, ZipPolicy policy, IZipArcvhiveService zipArcvhiveService, IOpenRead openRead, ILogger<FileScanner> logger)
        {

            _hasher = hasher;
            _policy = policy;
            _zipArcvhiveService = zipArcvhiveService;
            _openRead = openRead;
            _logger = logger;
        }

        public async Task ScanFileAsync(string path)
        {

            if (File.Exists(path))
            {              
                var hash = await _hasher.HashFunc(path);
                
                Console.WriteLine(hash);
                return;
            }


            path = Path.GetFullPath(path);
            Stack<string> dirs = new Stack<string>();
            dirs.Push(path);
            var visited = new HashSet<string>();

            while (dirs.Count > 0)
            {
                var currentDir = dirs.Pop();
                if (visited.Contains(currentDir)) continue;
                visited.Add(currentDir);
                try
                {
  
                    if (File.Exists(currentDir))
                    {

                        var hash = await _hasher.HashFunc(currentDir);
                        Console.WriteLine(hash);
                    }
                    else if (Directory.Exists(currentDir))
                    {
                        var subDirs = Directory.EnumerateDirectories(currentDir);

                        foreach (var subDir in subDirs)
                        {
                            dirs.Push(subDir);
                        }
                        

                        var files = Directory.EnumerateFiles(currentDir);
                        
                        foreach (var f in files)
                        {
                            try
                            {
                                var extension = Path.GetExtension(f).ToLower();
                                
                                if (extension == ".zip") await ProcessZipFileAsync(f);

                                var hash = await _hasher.HashFunc(f);
                                Console.WriteLine("Hashing " + hash);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Error hashing : " + ex.Message);
                            }
                        }
                        Debug.WriteLine(currentDir);
                    }
                    else
                    {
                        Debug.WriteLine($"{currentDir}");
                    }

                }
                catch (UnauthorizedAccessException uaex)
                {
                    _logger.LogWarning(uaex, "Error scanning directory : {Directory}", currentDir);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error scanning directory : '{Directory}'", currentDir);
                    throw new Exception($"Error scanning directory : '{currentDir}'", ex);
                }
            }

        }
        public async Task ProcessZipFileAsync(string file)
        {
            if (string.IsNullOrEmpty(file)) return;

            file = Path.GetFullPath(file);

            try
            {
                var fileInfo = new FileInfo(file);

                if (fileInfo.Length > _policy.MaxTotalUncompressed)
                {
                    // File is too big
                    _logger.LogWarning("Zip file '{File}' skipped because size {Size} exceeds policy {Max}", file, fileInfo.Length, _policy.MaxTotalUncompressed);
                    return;
                }

                var openRead = _openRead.OpenAsync(file);
                var entry = await _zipArcvhiveService.OpenZipArchive(openRead.Result);
                var tempRoot = _zipArcvhiveService.HandleTempRoot(file);
            }
            catch (InvalidDataException idex)
            {
                _logger.LogError(idex, "Invalid ZIP data in file '{File}'", file);
                throw new Exception($"Invalid ZIP data in file '{file}'", idex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed processing zip file '{File}'", file);
                throw new Exception($"Failed processing zip file '{file}'", ex);
            }
        }
    }
}
