using System.Security.Cryptography;
using System.Globalization;
using System.Text.Json;
using System.Diagnostics;

namespace ConcurSolutionz.Database
{
    public class Entry: FileDB
    {
        private MetaData metaData;
        public MetaData MetaData { 
            get{
                return metaData;
            } 
            set{
                if (File.Exists(Utilities.ConstEntryMetaDataPath(this.FilePath)))
                {
                    metaData = value ?? null;

                    // Create Entry MetaData
                    string json;
                    // Write over the current existing metaData JSON file
                    string entryMetaDataPath = Utilities.ConstEntryMetaDataPath(this.FilePath);
                    json = JsonSerializer.Serialize(MDAdaptor.ConvertMetaData(metaData));

                    File.WriteAllText(entryMetaDataPath, json);
                }

                else
                {
                    metaData = value;
                }
            }
        }
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
            FileType = builder.FileType;
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

            // Update last modified date of Entry
            UpdateModifiedDate();
        }


        /// <summary>Deletes a record from the database.</summary>
        /// <param name="record">The record to be deleted.</param>
        /// <remarks>
        /// This method removes the specified record from the database. It also deletes the corresponding JSON file
        /// containing the record's metadata and the folder containing the record's receipt.
        /// </remarks>
        public void DelRecord(Record record){
            // Check if record exist in Records List of Entry
            if (!Records.Contains(record))
            {
                throw new ArgumentException("While attempting to delete record from Entry," +
                    "the record argument does not exist in the Records List!");
            }

            // Remove record object from list of records for Entry
            Records.Remove(record);

            // Remove paths associated to this record
            // Paths required as arguments to populate receipt folder (Update it with new receipt)
            string receiptFolderPath = Utilities.ConstReceiptsFdrPath(FilePath);
            string receiptJSONPath = Utilities.ConstReceiptMetaDataPath(FilePath);

            Database.DeleteFile(Path.Combine(receiptJSONPath, record.RecordID + ".json")); // Del Metadata
            Database.DeleteFile(Path.Combine(receiptFolderPath,"Receipt " + record.RecordID.ToString() + ".jpg")); // Del Receipt Image
            Database.DeleteFile(Path.Combine(receiptFolderPath,"Receipt " + record.RecordID.ToString() + ".png")); // Del Receipt Image


            // Update last modified date of Entry
            UpdateModifiedDate();
        }


        /// <summary>Deletes a record from the list of Records by its ID.</summary>
        /// <param name="ID">The ID of the record to be deleted.</param>
        public void DelRecordByID(int ID){
            // Remove paths associated to this record
            // Paths required as arguments to populate receipt folder (Update it with new receipt)
            string receiptFolderPath = Utilities.ConstReceiptsFdrPath(FilePath);
            string receiptJSONPath = Utilities.ConstReceiptMetaDataPath(FilePath);
            
            foreach (Record record in Records){
                if (record.RecordID == ID){
                    Records.Remove(record);
                    Database.DeleteFile(Path.Combine(receiptJSONPath, record.RecordID + ".json")); // Del Metadata
                    Database.DeleteFile(Path.Combine(receiptFolderPath, "Receipt " + record.RecordID.ToString() + ".jpg")); // Del Receipt Image
                    Database.DeleteFile(Path.Combine(receiptFolderPath, "Receipt " + record.RecordID.ToString() + ".png")); // Del Receipt Image

                    // Update last modified date of Entry
                    UpdateModifiedDate();
                    
                    return; // Assume there is only one instance UNIQUE RecordID 
                }
            }
            // If method reaches here, means we have not found any record of ID passed
            throw new ArgumentException("While attempting to delete record from Entry using an ID," +
                    "the record (Based on RecordID) does not exist in the Records List!");
        }

        /// <summary>Returns a list of Records.</summary>
        /// <returns>A list of Records.</returns>
        public List<Record> GetRecords(){
            return Records;
        }


        /// <summary>Retrieves a record with the specified ID.</summary>
        /// <param name="ID">The ID of the record to retrieve.</param>
        /// <returns>The record with the specified ID. (Null if no record found in Entry Instance)</returns>
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
            // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
            // TODO: Call EntrySubSystem to add records (FOR EXISTING ENTRY)
            // I.e. when Entry exist in File Management already... Then we want to
            // edit or modify it.
            // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

            return;
        }


        /// <summary>Returns a unique record ID for a receipt (Particular Entry).</summary>
        /// <returns>The assigned record ID.</returns>
        private int AssignRecordID(){
            string ReceiptMetaDataPath = Utilities.ConstReceiptMetaDataPath(FilePath);

            // Get all files with .json file extension in the ReceiptMetaData Folder.
            string[] ReceiptMetaDatas = Directory.GetFiles(Path.Combine(ReceiptMetaDataPath,""), "*.json");
            int assignedIndex = 0;

            // Increment assignedIndex and check for each iteration if number has been used
            // If "<assignedIndex>.json" exist in the list of receipt metadata files.
            foreach (string path in ReceiptMetaDatas)
            {
                if (Path.GetFileName(path) == $"{assignedIndex}.json")
                {
                    assignedIndex++;
                }
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
            public string FileType { get; private set; }

            public EntryBuilder(){
                // Set Default Values
                Records = new List<Record>();

                // Set FileType (to be used by adaptor)
                FileType = "Entry";
            }

            public EntryBuilder SetFileName(string FileName){
                Utilities.CheckIfEmptyString(FileName);
                this.FileName = FileName + ".entry";
                return this;
            }

            public EntryBuilder SetCreationDate(DateTime CreationDate){
                Utilities.CheckDateTimeAheadOfNow(CreationDate);
                this.CreationDate = CreationDate;
                return this;
            }

            public EntryBuilder SetLastModifiedDate(DateTime LastModifiedDate){
                Utilities.CheckDateTimeAheadOfNow(LastModifiedDate);
                Utilities.CheckLastModifiedAheadOfCreation(LastModifiedDate, CreationDate);
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
                if (!Directory.Exists(FilePath))
                {
                    throw new IOException("Directory does not exist");
                }

                this.FilePath = Path.Combine(FilePath, FileName);

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