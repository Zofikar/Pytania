using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pytania.Models
{
    public class FileNamesConnection
    {
        public FileNamesConnection(string FilePath) 
        { 
            this.FilePath = FilePath;
        }
        public string FilePath {get; set;}
        public string FileExension { get { return Path.GetExtension(FilePath); } }
        public string FileName { get { return Path.GetFileName(FilePath); } }
        public string FileNameWithoutExtension { get { return Path.GetFileNameWithoutExtension(FilePath); } }
    }
}
