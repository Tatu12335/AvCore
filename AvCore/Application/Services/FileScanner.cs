
using AvCore.Application.Interfaces;
using AvCore.Domain.Entities.policies;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AvCore.Application.Services
{
    public class FileScanner : IFileScanner
    {
        private readonly IHasher _hasher;
        private readonly ZipPolicy _policy;
        private readonly IZipArcvhiveService _zipArcvhiveService;
        private readonly IOpenRead _openRead;
        private readonly ILogger<FileScanner> _logger;

        public FileScanner( IHasher hasher,ZipPolicy policy,IZipArcvhiveService zipArcvhiveService,IOpenRead openRead, ILogger<FileScanner> logger)
        {
            
            _hasher = hasher;
            _policy = policy;
            _zipArcvhiveService = zipArcvhiveService;
            _openRead = openRead;
            _logger = logger;
        }

        public async Task ScanFileAsync(string path)
        {
            path = Path.GetFullPath(path);
            Stack<string> dirs = new Stack<string>();
            dirs.Push(path);
            Debug.WriteLine("MATAFAKA");

            while (dirs.Count > 0)
            {
                var currentDir = dirs.Pop();

                try
                {
                    var files = Directory.EnumerateFiles(currentDir);

                    foreach (var file in files)
                    {
                        var ext = Path.GetExtension(file).ToLower();

                        if (ext == ".zip")
                        {
                            await ProcessZipFileAsync(file);
                            break;
                        }
                        else
                        {
                            FileInfo fileInfo = new FileInfo(file);

                            var hash = await _hasher.HashFunc(fileInfo);
                            Debug.WriteLine("TOIMI PERKELE2");

                            if (string.IsNullOrEmpty(hash))
                            {
                                _logger.LogError("Hasher returned null/empty for file : '{FilePath}' ", fileInfo.FullName);
                                throw new InvalidOperationException($"Hasher returned null/empty for file '{fileInfo.FullName}'.");
                            }
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
                                var hash = await _hasher.HashFunc(fileInfo);
                                Debug.WriteLine("TOIMI PERKELE");

                                if (string.IsNullOrEmpty(hash))
                                {
                                    _logger.LogError("Hasher returned null/empty for file : '{FilePath}'", fileInfo.FullName);
                                    throw new InvalidOperationException($"Hasher returned null/empty for file '{fileInfo.FullName}'.");
                                }
                            }
                        }

                        dirs.Push(dir);
                    }
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
