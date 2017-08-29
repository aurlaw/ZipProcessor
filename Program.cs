using System;
using System.Linq;
using System.IO.Compression;
using System.IO;

namespace zip_processor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Hello {System.Environment.GetEnvironmentVariable("USER")}! I'm {System.Environment.MachineName} and I'm talking to you from {System.IO.Directory.GetCurrentDirectory()}");

            if(args.Any()) {
                var zipPath = args.First();
                if(zipPath.EndsWith(".zip") && File.Exists(zipPath)) 
                {
                    try {
                        using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Read))
                            {
                                foreach(var entry in archive.Entries) {
                                    Console.WriteLine(entry.FullName);
                                }
                            } 
                    }
                    catch(Exception ex) {
                        Console.WriteLine(ex.ToString());
                    }
                } else {
                        Console.WriteLine("no valid zip file provided");

                }
            } else {
                Console.WriteLine("no file provided");
            }

            Console.ReadLine();
        }
    }
}
