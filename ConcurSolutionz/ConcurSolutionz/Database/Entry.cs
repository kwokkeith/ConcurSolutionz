
namespace ConcurSolutionz.Database
{
    public class Entry: FileDB
    {
        public MetaData MetaData { get; set; }
        private  List<Record> Records { get; set; }


        private Entry(EntryBuilder builder){
            // Check if attributes have been declared (Mandatory)
            Utilities.CheckNull(builder.FileName);
            Utilities.CheckNull(builder.CreationDate);
            Utilities.CheckNull(builder.LastModifiedDate);
            Utilities.CheckNull(builder.FilePath);
            Utilities.CheckNull(builder.MetaData);
            Utilities.CheckNull(builder.Records);

            // Set attributes
            FileName = builder.FileName;
            CreationDate = builder.CreationDate;
            LastModifiedDate = builder.LastModifiedDate;
            FilePath = builder.FilePath;
            MetaData = builder.MetaData;
            Records = builder.Records;
            SetFolder();
        }


        // Set mandatory boolean of File Instance
        protected override void SetFolder(){
            this.Folder = false;
        }

        /// <summary>Adds a record to the list of Records.</summary>
        /// <param name="record">The record to be added.</param>
        public void AddRecord(Record record){
            Records.Add(record);
        }


        /// <summary>Deletes a record from the list of Records.</summary>
        /// <param name="record">The record to be deleted.</param>
        public void DelRecord(Record record){
            Records.Remove(record);
        }

        /// <summary>Deletes a record from the list of Records by its ID.</summary>
        /// <param name="ID">The ID of the record to be deleted.</param>
        public void DelRecordByID(int ID){
            Records.RemoveAll(record => record.GetRecordID() == ID);
        }

        /// <summary>Returns a list of Records.</summary>
        /// <returns>A list of Records.</returns>
        public List<Record> GetRecords(){
            return Records;
        }


        /// <summary>Retrieves a record with the specified ID.</summary>
        /// <param name="ID">The ID of the record to retrieve.</param>
        /// <returns>The record with the specified ID.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no record with the specified ID is found.</exception>
        public Record GetRecord(int ID){
            foreach (Record record in Records){
                if (record.GetRecordID() == ID){
                    return record;
                }
            }
            return null;
        }


        // Calls EntrySubsystem
        public override void SelectedAction(){
            // TODO: Call EntrySubSystem to add entry
            return;
        }


        // Builder Class for Entry
        public class EntryBuilder
        {
            public string FileName { get; private set; }
            public DateTime CreationDate { get; private set; }
            public DateTime LastModifiedDate { get; private set; }
            public string FilePath { get; private set; }
            public MetaData MetaData { get; private set; }
            public List<Record> Records { get; private set; }    

            EntryBuilder(){
                // Set Default Values
                Records = new List<Record>();
            }

            public EntryBuilder SetFileName(string FileName){
                this.FileName = FileName;
                return this;
            }

            public EntryBuilder SetCreationDate(DateTime CreationDate){
                this.CreationDate = CreationDate;
                return this;
            }

            public EntryBuilder SetLastModifiedDate(DateTime LastModifiedDate){
                this.LastModifiedDate = LastModifiedDate;
                return this;
            }

            public EntryBuilder SetFilePath(string FilePath){
                this.FilePath = FilePath;
                return this;
            }

            public EntryBuilder SetMetaData(MetaData MetaData){
                this.MetaData = MetaData;
                return this;
            }

            public EntryBuilder SetRecords(List<Record> Records){
                this.Records = Records;
                return this;
            }

            public Entry Build(){
                return new Entry(this);
            }
        }
    }
}