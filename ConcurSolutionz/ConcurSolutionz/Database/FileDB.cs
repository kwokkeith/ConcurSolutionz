
namespace ConcurSolutionz.Database
{
    public abstract class FileDB
    {
        public string FileName { get; set; }
        private DateTime creationDate;
        public string FileType { get; set; }
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                Utilities.CheckDateTimeAheadOfNow(value);
                creationDate = value;
            }
        }
        private DateTime lastModifiedDate;
        public DateTime LastModifiedDate
        {
            get
            {
                return lastModifiedDate;
            }
            set
            {
                Utilities.CheckDateTimeAheadOfNow(value);
                lastModifiedDate = value;
            }
        }
        public string FilePath { get; set; }
        public bool Folder;


        protected abstract void SetFolder();


        /// <summary>Checks if a file is a folder in the database.</summary>
        /// <return>true if file is a folder, else false.</return>
        public bool IsFolder()
        {
            Utilities.CheckNull(Folder); // Check if folder has been set
            return Folder;  
        }


        public abstract void SelectedAction();

        public void UpdateModifiedDate()
        {
            LastModifiedDate = DateTime.Now;
        }
    }
}