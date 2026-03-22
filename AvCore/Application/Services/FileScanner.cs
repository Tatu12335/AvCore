
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
        public FileScanner(IHasher hasher, ZipPolicy policy, IZipArcvhiveService zipArcvhiveService, IOpenRead openRead)
        {
            _hasher = hasher;
            _policy = policy;
            _zipArcvhiveService = zipArcvhiveService;
            _openRead = openRead;
        }
        public async Task ScanFileAsync(string path)
        {
            path = Path.GetFullPath(path);
            Stack<string> dirs = new Stack<string>();
            dirs.Push(path);
            Debug.WriteLine("MATAFAKA");

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
                                throw new InvalidOperationException($"Hasher returned null/empty for file '{fileInfo.FullName}'.");

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
                                    throw new InvalidOperationException($"Hasher returned null/empty for file '{fileInfo.FullName}'.");

                            }

                        }

                        dirs.Push(dir);

                    }
                }
                catch (Exception ex)
                {
                    // Preserve original exception as InnerException so stack trace and type are not lost
                    throw new Exception($"Error scanning directory '{currentDir}'", ex);
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
                    return;
                }
                var openRead = _openRead.OpenAsync(file);
                var entry = await _zipArcvhiveService.OpenZipArchive(openRead.Result);
                var tempRoot = _zipArcvhiveService.HandleTempRoot(file);
            }
            catch (InvalidDataException idex)
            {
                throw new Exception($"Invalid ZIP data in file '{file}'", idex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed processing zip file '{file}'", ex);
            }

        }

    } 
}

