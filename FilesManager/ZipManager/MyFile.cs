using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipManager
{
    public class MyFile
    {
        public string FileType
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string Path
        {
            get; set;
        }

        public DateTime LastWriteTime
        {
            get; set;
        }

        public override string ToString()
        {
            return $"{FileType}\t{Name}\t{LastWriteTime}";
        }
    }
}
