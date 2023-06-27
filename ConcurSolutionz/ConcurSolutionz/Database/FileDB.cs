using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurSolutionz.Database
{
    public abstract class FileDB
    {
        private string fileName { get; set; };
        private DateTime creationDate { get; set; }
        private DateTime lastModifiedDate { get; set; }
        private String filePath { get; set; }
        private bool folder;

        private abstract void setFolder();

        public bool isFolder(){
            Utilities.checkNull(folder); // Check if folder has been set
            return folder;  
        }

        public abstract void selectedAction();
    }
}