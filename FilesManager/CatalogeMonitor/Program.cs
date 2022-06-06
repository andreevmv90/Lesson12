using System;
using System.Collections.Generic;
using System.IO;
using ZipManager;

namespace CatalogeMonitor
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello! This is Monitor");
            var file = DefaultDirectory + Path.DirectorySeparatorChar + "Lesson12Homework.txt";
            if (!File.Exists(file))
            {
                Console.WriteLine($"File {file} is not found");
            }
            else
            {
                Console.WriteLine($"Opening file {file}");

                var monitor = new Monitor()
                {
                    WorkingCataloge = DefaultDirectory,
                    SetFilePath = file
                };
                if (monitor.ReadData())
                {
                    Console.WriteLine($"File {Path.GetFileNameWithoutExtension(file)} includes : ");
                    foreach (MyFile myfile in monitor.MyFiles)
                    {
                        Console.WriteLine(myfile);
                    }
                    monitor.DeleteSetFile();
                }
            }
            Console.ReadKey();
        }


        /// <summary>
        /// Директория файлов проекта по умолчанию
        /// </summary>
        private static string DefaultDirectory
        {
            get { return Directory.GetCurrentDirectory(); }
        }
    }
}
