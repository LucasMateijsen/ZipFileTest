using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace ZipFileTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the current base directory
            var path = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent;
            // Define the source and the zip dir
            var sourceDir = path.GetDirectories("sourceFiles").First();
            var zippedDir = path.GetDirectories("zippedFiles").First();
            // Generate the zipname
            string zipName = "Export_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".zip";

            // Create a memoryStream to hold the zipfile
            using (var memoryStream = new MemoryStream())
            {
                // Create the archive
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    // Loop trough the files to add
                    foreach (var sourceFile in sourceDir.GetFiles())
                    {
                        // Create a new entry
                        var zippedFile = archive.CreateEntry(sourceFile.Name, CompressionLevel.Optimal);
                        // Copy the contents into the new entry
                        using (var entryStream = zippedFile.Open())
                        {
                            using (var fileToCompressStream = new MemoryStream(File.ReadAllBytes(sourceFile.FullName)))
                            {
                                fileToCompressStream.CopyTo(entryStream);
                            }
                        }
                    }
                }

                // Created a filestream here, but could also be a memory stream
                using (var fileStream = new FileStream($"{zippedDir.FullName}\\{zipName}", FileMode.Create))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    memoryStream.CopyTo(fileStream);
                }
            }
        }
    }
}
