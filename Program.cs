using System;
using System.Linq;
using System.IO.Compression;
using System.IO;
using System.Text;
using System.Reflection;
namespace STZipProcessor
{
    class Program
    {
        private static string[] choiceArray = new[] {"rate", "rates", "ExpressNet_AccountsTermsConditions", "Other" };

        static void Main(string[] args)
        {
            Console.WriteLine($"Zip Processor {Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}");
            if (!args.Any())
            {
                Console.WriteLine("No file provided");
                Console.ReadLine();
                return;
            }
            var arg1 = args.First();
            try
            {
                var zproc = new ZipProcessor(arg1);
                zproc.Process(z =>
                {
                    var entityName = string.Empty;
                    do
                    {
                        ShowPrompt();
                        Reset();
                        var choice = 0;
                        if (!int.TryParse(Console.ReadLine().Trim(), out choice)) continue;
                        if (choice > -1 && choice < choiceArray.Length)
                        {
                            entityName = choice == choiceArray.Length - 1 ? PromptFile() : choiceArray[choice];
                        }
                    } while (string.IsNullOrEmpty(entityName));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Using name '{0}'", entityName);
                    Reset();
                    return entityName;
                });
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Generated archive {0}", zproc.NewArchivePath);
            }
            catch (Exception exception)
            {
                Reset();
                Console.WriteLine(exception.ToString());
            }
            finally
            {
                Reset();
                Console.ReadLine();
            }
            // if(args.Any()) {
            //     var zipPath = args.First();
            //     if(zipPath.EndsWith(".zip") && File.Exists(zipPath)) 
            //     {
            //         try {
            //             using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Read))
            //                 {
            //                     foreach(var entry in archive.Entries) {
            //                         Console.WriteLine(entry.FullName);
            //                     }
            //                 } 
            //         }
            //         catch(Exception ex) {
            //             Console.WriteLine(ex.ToString());
            //         }
            //     } else {
            //             Console.WriteLine("no valid zip file provided");

            //     }
            // } else {
            //     Console.WriteLine("no file provided");
            // }

            Console.ReadLine();
        }

                static void ShowPrompt()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            var display = new StringBuilder();
            display.AppendLine("Please choose from the following to determine the name of the file to be used.");
            display.AppendLine("Note: the file extension will be used from the original file");
            for (var x=0; x<choiceArray.Length;x++)
            {
                display.AppendFormat("{0} - {1}", x, choiceArray[x]).AppendLine();
            }
            Console.Write(display);
            Console.Write("Enter a number between 0 and {0}: ", choiceArray.Length-1);

        }

        static string PromptFile()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Please enter a filename without extension. i.e rate: ");
            var result = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(result))
                PromptFile();
            else
            {
                if (result.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    PromptFile();
            }
            return result;
        }
        private static void Reset()
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
