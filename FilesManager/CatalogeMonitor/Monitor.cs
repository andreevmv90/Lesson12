using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZipManager;

namespace CatalogeMonitor
{
    public class Monitor
    {
        /// <summary>
        /// Рабочий каталог
        /// </summary>
        public string WorkingCataloge
        {
            get; set;
        }

        /// <summary>
        /// Путь к настроечному файлу
        /// </summary>
        public string SetFilePath
        {
            get; set;
        }

        /// <summary>
        /// Путь к файлу с данными
        /// </summary>
        public string DataFilePath
        {
            get; private set;
        }

        /// <summary>
        /// Лист с файлами
        /// </summary>
        public List<MyFile> MyFiles
        {
            get; private set;
        }

        /// <summary>
        /// Чтение файла с информацией по файлам
        /// </summary>
        /// <returns></returns>
        public bool ReadData()
        {
            if (ReadSetFile())
            {
                MyFiles = ReadDataFile();
                return true;
            }
            return false;
        }

        private bool ReadSetFile()
        {
            if (string.IsNullOrEmpty(SetFilePath))
                return false;
            using (FileStream fs = File.OpenRead(SetFilePath))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    DataFilePath = sr.ReadLine();
                }
            }
            if (string.IsNullOrEmpty(DataFilePath))
                return false;
            return true;
        }

        private List<MyFile> ReadDataFile()
        {
            var list = new List<MyFile>();
            using (FileStream fs = File.OpenRead(DataFilePath))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    var inputData = "";
                    while (inputData != null)
                    {
                        inputData = sr.ReadLine();
                        if (inputData != null)
                        {
                            string[] values = inputData.Split('\t');

                            if (values.Length == 3)
                            {
                                DateTime time;
                                if (!DateTime.TryParse(values[2], out time))
                                    time = new DateTime();
                                var myfile = new MyFile
                                {
                                    FileType = values[0],
                                    Name = values[1],
                                    LastWriteTime = time
                                };
                                list.Add(myfile);
                            }
                        }
                        
                    }                  
                }
            }
            return list;
        }

        /// <summary>
        /// Удаление файла настроек (который содержит путь к основному файлу)
        /// </summary>
        /// <returns></returns>
        public bool DeleteSetFile()
        {
            if (File.Exists(SetFilePath))
            {
                File.Delete(SetFilePath);
                return true;
            }
            return false;
        }
    }
}
