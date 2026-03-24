
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
        private readonly IAbuseClient _abuseClient;
        private readonly IHandleResults _handleResults;

        public FileScanner(IHasher hasher, ZipPolicy policy, IZipArcvhiveService zipArcvhiveService, IOpenRead openRead, ILogger<FileScanner> logger, IHandleResults handleResults,IAbuseClient abuseClient)
        {

            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
            _zipArcvhiveService = zipArcvhiveService ?? throw new ArgumentNullException(nameof(zipArcvhiveService));
            _openRead = openRead ?? throw new ArgumentNullException(nameof(openRead));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _abuseClient = abuseClient ?? throw new ArgumentNullException(nameof(abuseClient)); 
            _handleResults = handleResults ?? throw new ArgumentNullException(nameof(handleResults));
        }
    

        // SORRY for the spaghetti :(
        public async Task<string> ScanFileAsync(string path)
        {

            if (File.Exists(path))
            {              
                var hash = await _hasher.HashFunc(path);
                
                Console.WriteLine(hash);

                var response  = await _abuseClient.GetAbuseChClient(hash);

                if (response == null) throw new Exception("API Response is null!");

                var handled = await _handleResults.HandleResult(response);
                return handled;
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

                                if (hash == null) return "";
                                 
                                var response = await _abuseClient.GetAbuseChClient(hash);

                                if (response == null) throw new Exception("API Response is null!");
                                
                                var handled = await _handleResults.HandleResult(response);

                                Console.WriteLine(handled);
                                

                                
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Error hashing : " + ex.Message);
                            }
                        }
                        
                    }
                    else
                    {
                        Debug.WriteLine($"{currentDir}");
                        return null;
                    }

                }
                catch (UnauthorizedAccessException uaex)
                {
                    _logger.LogWarning(uaex, "Error scanning directory : {Directory}", currentDir);
                    return null;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error scanning directory : '{Directory}'", currentDir);
                    throw new Exception($"Error scanning directory : '{currentDir}'", ex);
                }
            }
            return null;
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

                var openRead = await _openRead.OpenAsync(file);
                var entry = await _zipArcvhiveService.OpenZipArchive(openRead);
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
