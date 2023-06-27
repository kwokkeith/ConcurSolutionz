
namespace ConcurSolutionz.Database
{
    public abstract class FileDB
    {
        public string fileName { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public String filePath { get; set; }
        public bool folder;

        protected abstract void setFolder();

        public bool isFolder(){
            Utilities.checkNull(folder); // Check if folder has been set
            return folder;  
        }

        public abstract void selectedAction();
    }
}