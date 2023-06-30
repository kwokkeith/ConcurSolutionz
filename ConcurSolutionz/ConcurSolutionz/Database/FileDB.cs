
namespace ConcurSolutionz.Database
{
    public abstract class FileDB
    {
        public string FileName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string FilePath { get; set; }
        public bool Folder;

        protected abstract void SetFolder();

        public bool IsFolder(){
            Utilities.CheckNull(Folder); // Check if folder has been set
            return Folder;  
        }

        public abstract void SelectedAction();

        public void UpdateModifiedDate(){
            LastModifiedDate = DateTime.Now;
        }
    }
}