using System;
using System.IO;
using System.IO.Compression;

namespace ZipManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello! This is Archive Manager");
            var acrchiveManager = new ArchiveManager() 
            {
                WorkingCataloge = DefaultDirectory, 
                WorkingType = "zip" 
            };

            if (!acrchiveManager.FindArchive())
                Console.WriteLine("There are no acrchives . See log :");
            Console.WriteLine(acrchiveManager.GetLog);

            if (!acrchiveManager.ExtractArchive(0))
                Console.WriteLine($"Acrchive {acrchiveManager.WorkingArchive} extraction problem . See log :");
            Console.WriteLine(acrchiveManager.GetLog);

            if (!acrchiveManager.WriteInfo())
                Console.WriteLine($"Acrchive {acrchiveManager.WorkingArchive} analizing problem . See log :");
            acrchiveManager.DeleteWorkingArchive();
            acrchiveManager.WriteWorkingArchivePath();

            Console.WriteLine($"Работа с архивом {acrchiveManager.WorkingArchive} закончена");
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
