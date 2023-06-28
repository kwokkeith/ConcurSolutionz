using System.Globalization;

namespace ConcurSolutionz.Database
{
    public class Entry: FileDB
    {
        public MetaData MetaData { get; set; }
        public List<Record> Records { get; set; }


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
            // Update the RecordID
            int nextID = AssignRecordID();
            record.RecordID = nextID;

            // Add record object to list of records for Entry
            Records.Add(record);

            // Paths required as arguments to populate receipt folder (Update it with new receipt)
            string receiptFolderPath = Utilities.ConstReceiptsFdrPath(FilePath);
            string receiptJSONPath = Utilities.ConstReceiptMetaDataPath(FilePath);

            FileCreator.PopulateReceiptFolder(this, receiptFolderPath, receiptJSONPath);
        }

        /// <summary>Deletes a record from the database.</summary>
        /// <param name="record">The record to be deleted.</param>
        /// <remarks>
        /// This method removes the specified record from the database. It also deletes the corresponding JSON file
        /// containing the record's metadata and the folder containing the record's receipt.
        /// </remarks>
        public void DelRecord(Record record){
            
            // Remove record object from list of records for Entry
            Records.Remove(record);

            // Remove paths associated to this record
            // Paths required as arguments to populate receipt folder (Update it with new receipt)
            string receiptFolderPath = Utilities.ConstReceiptsFdrPath(FilePath);
            string receiptJSONPath = Utilities.ConstReceiptMetaDataPath(FilePath);

            Database.DeleteFile(receiptJSONPath + "\\" + record.RecordID + ".json"); // Del Metadata
            Database.DeleteFile(receiptFolderPath + "\\" + record.RecordID); // Del Receipt Image
        }


        /// <summary>Deletes a record from the list of Records by its ID.</summary>
        /// <param name="ID">The ID of the record to be deleted.</param>
        public void DelRecordByID(int ID){
            Records.RemoveAll(record => record.RecordID == ID);
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
                if (record.RecordID == ID){
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


        /// <summary>Returns a unique record ID for a receipt (Particular Entry).</summary>
        /// <returns>The assigned record ID.</returns>
        private int AssignRecordID(){
            string ReceiptMetaDataPath = Utilities.ConstReceiptMetaDataPath(FilePath);
            string[] ReceiptMetaDatas = Directory.GetFiles(ReceiptMetaDataPath + "\\", "*.json");
            int assignedIndex = 0;
            while (ReceiptMetaDatas.Contains(Convert.ToString(assignedIndex) + ".json")){
                assignedIndex++;
            }
            return assignedIndex;
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
                this.FileName = FileName + ".entry";
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

            /// <summary>Sets the file path for the entry being built.</summary>
            /// <param name="FilePath">The file path to set (working directory).</param>
            /// <returns>The updated EntryBuilder instance.</returns>
            /// <exception cref="ArgumentNullException">Thrown when the FileName is null.</exception>
            public EntryBuilder SetFilePath(string FilePath){
                // Makes use of working directory of database to create a file
                Utilities.CheckNull(FileName);
                this.FilePath = FilePath + "\\" + FileName;
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