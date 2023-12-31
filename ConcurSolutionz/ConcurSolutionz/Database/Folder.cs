namespace ConcurSolutionz.Database
{
    public class Folder: FileDB
    {
        private Folder(FolderBuilder builder)
        {
            // Check if attributes have been declared (Mandatory)
            Utilities.CheckIfValidName(builder.FileName);
            Utilities.CheckNull(builder.CreationDate);
            Utilities.CheckNull(builder.LastModifiedDate);
            Utilities.CheckNull(builder.FilePath);

            // Set attributes
            FileName = builder.FileName;
            CreationDate = builder.CreationDate;
            LastModifiedDate = builder.LastModifiedDate;
            FilePath = builder.FilePath;
            FileType = GetType().FullName;
            SetFolder();
        }


        ///<summary>Set mandatory boolean of File Instance</summary>
        protected override void SetFolder()
        {
            Folder = true;
        }


        /// <summary>Action to be called when file is selected</summary>
        public override void SelectedAction()
        {
            StepIntoFolder();
            return;
        }


        /// <summary>Action called to step into a folder.</summary>
        /// <remarks>Updates the current working directory to the folder's directory.</remarks>
        private void StepIntoFolder()
        {
            try
            {
                Database.Instance.Setwd(FilePath);

            }
            catch (Exception ex){
                Console.WriteLine(ex.ToString());
            }
        }


        // Builder Class for Folder
        public class FolderBuilder
        {
            public string FileName { get; private set; }
            public DateTime CreationDate { get; private set; }
            public DateTime LastModifiedDate { get; private set; }
            public string FilePath { get; private set; }


            public FolderBuilder()
            {
                // Set Default Values
            }


            public FolderBuilder SetFileName(string FileName)
            {
                Utilities.CheckIfEmptyString(FileName);
                this.FileName = FileName + ".fdr";
                return this;
            }


            public FolderBuilder SetCreationDate(DateTime CreationDate)
            {
                Utilities.CheckDateTimeAheadOfNow(CreationDate);
                this.CreationDate = CreationDate;
                return this;
            }


            public FolderBuilder SetLastModifiedDate(DateTime LastModifiedDate)
            {
                Utilities.CheckDateTimeAheadOfNow(LastModifiedDate);
                Utilities.CheckLastModifiedAheadOfCreation(LastModifiedDate, CreationDate);
                this.LastModifiedDate = LastModifiedDate;
                return this;
            }


            /// <summary>Sets the file path for the folder being built.</summary>
            /// <param name="FilePath">The file path to set (working directory).</param>
            /// <returns>The updated FolderBuilder instance.</returns>
            /// <exception cref="ArgumentNullException">Thrown when the FileName is null.</exception>
            public FolderBuilder SetFilePath(string FilePath)
            {
                Utilities.CheckNull(FileName);
                this.FilePath = Path.Combine(FilePath, FileName);

                return this;
            }


            public Folder Build()
            {
                return new Folder(this);
            }
        }
        
        
    }
}