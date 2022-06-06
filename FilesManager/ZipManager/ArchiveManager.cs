using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipManager
{
    public class ArchiveManager
    {
        /// <summary>
        /// Рабочий каталог
        /// </summary>
        public string WorkingCataloge
        {
            get; set;
        }

        /// <summary>
        /// Рабочий тип архива
        /// </summary>
        public string WorkingType
        {
            get; set;
        }

        /// <summary>
        /// Полный список архивных файлов указанного типа в указанном рабочем каталоге
        /// </summary>
        public string[] Archives
        {
            get; private set;
        }

        /// <summary>
        /// Рабочий архив
        /// </summary>
        public string WorkingArchive
        {
            get; private set;
        }

        /// <summary>
        ///  Каталог, в котором хранятся распакованные файлы
        /// </summary>
        public string ExtcractedCataloge
        {
            get; private set;
        }

        /// <summary>
        /// Полный список файлов в рабочем архиве
        /// </summary>
        public List<MyFile> ExtcractedObjects
        {
            get; private set;
        }

        /// <summary>
        /// Ведет журнал сообщений по работе с архивом
        /// </summary>
        private StringBuilder Log;

        /// <summary>
        /// Возвращает сообщение Log
        /// </summary>
        public string GetLog
        {
            get
            {
                return Log.ToString();
            }
        }
      
        public string PathToFile
        {
            get; set;
        }

        // конструктор
        public ArchiveManager()
        {
            Log = new StringBuilder();
            ExtcractedObjects = new List<MyFile>();
        }


        /// <summary>
        /// Метод поиска архива в каталоге проекта
        /// </summary>
        /// <returns></returns>
        public bool FindArchive()
        {
            // очищаем Log 
            Log?.Clear();

            if (string.IsNullOrEmpty(WorkingType) || string.IsNullOrEmpty(WorkingCataloge))
            {
                Log.AppendLine("Не задан рабочий каталог или тип архива");
                return false;
            }

            if (!Directory.Exists($"{WorkingCataloge}"))
            {
                Log.AppendLine("Рабочий каталог не существует");
                return false;
            }

            var archives = Directory.GetFiles(WorkingCataloge, "*."+ WorkingType, SearchOption.TopDirectoryOnly);
            if (archives.Length == 0)
            {
                Log.AppendLine("Архивы не найдены в рабочем каталоге");
                return false;
            }

            Archives = archives;
            Log.AppendLine("Ок! Архивы найдены");
            return true;
        }

        /// <summary>
        /// Метод извеления файлов из выбранного каталога
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool ExtractArchive(int index)
        {
            // очищаем Log 
            Log?.Clear();
            ExtcractedObjects = new List<MyFile>(); 

            if (Archives == null || Archives.Length == 0)
            {
                Log.AppendLine("Нет архивов в рабочем каталоге");
                return false;
            }
            if (index < 0)
            {
                Log.AppendLine($"Указанный индекс {index} некорректен");
                return false;
            }
            WorkingArchive = Archives[index];

            var name = Path.GetFileNameWithoutExtension(WorkingArchive);

            Log.AppendLine($"Выбран архив : {name}");

            ZipFile.ExtractToDirectory(WorkingArchive, WorkingCataloge, true);
            ExtcractedCataloge = WorkingCataloge + Path.DirectorySeparatorChar + name;

            Log.AppendLine($"Архив содержит {Directory.GetFileSystemEntries(ExtcractedCataloge).Length} объектов");

            foreach (var obj in Directory.GetFileSystemEntries(ExtcractedCataloge))
            {
                bool isDirectory = string.IsNullOrEmpty(Path.GetExtension(obj));
          
                var myfile = new MyFile
                {
                    Name = Path.GetFileNameWithoutExtension(obj),
                    Path = obj,
                    FileType = isDirectory ? "Directory" : "File",
                    LastWriteTime = isDirectory ? Directory.GetCreationTime(obj): File.GetLastWriteTime(obj)
                };

                ExtcractedObjects.Add(myfile);
            }

            return true;
        }

        /// <summary>
        /// Запись по распаковке в файл
        /// </summary>
        /// <returns></returns>
        public bool WriteInfo()
        {
            PathToFile = WorkingCataloge + Path.DirectorySeparatorChar + "Lesson12Homework.cvs"; 
            using (FileStream fs = File.Create(PathToFile))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    for (var i = 0; i < ExtcractedObjects.Count; i++)
                    {
                        var myfile = ExtcractedObjects[i];
                        sw.WriteLine($"{myfile.FileType} \t {myfile.Name} \t {myfile.LastWriteTime}");
                    }

                }
            }

            return true;
        }

        /// <summary>
        /// Метод удаления рабочего архива
        /// </summary>
        /// <returns></returns>
        public bool DeleteWorkingArchive()
        {
            if (File.Exists(WorkingArchive))
            {
                File.Delete(WorkingArchive);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Записываем путь к рабочему архиву
        /// </summary>
        public void WriteWorkingArchivePath()
        {
            var path = WorkingCataloge + Path.DirectorySeparatorChar + "Lesson12Homework.txt";
            using (FileStream fs = File.Create(path))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(PathToFile);
                }
            }
        }
    }
}
