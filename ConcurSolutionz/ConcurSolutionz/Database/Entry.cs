using System.Reflection.PortableExecutable;
using System.Text;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Collections.Immutable;
using System.Globalization;
using System.Resources;
using System.Reflection.Metadata.Ecma335;
using System.Data.Common;
using System.ComponentModel;
using System.Data;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurSolutionz.Database
{
    public class Entry: FileDB
    {
        private MetaData metaData { get; set; }
        private List<Record> records;


        private Entry(EntryBuilder builder){
            // Check if attributes have been declared (Mandatory)
            Utilities.checkNull(builder.fileName);
            Utilities.checkNull(builder.creationDate);
            Utilities.checkNull(builder.lastModifiedDate);
            Utilities.checkNull(builder.filePath);
            Utilities.checkNull(builder.metaData);
            Utilities.checkNull(builder.records);

            // Set attributes
            fileName = builder.fileName;
            creationDate = builder.creationDate;
            lastModifiedDate = builder.lastModifiedDate;
            filePath = builder.filePath;
            metaData = builder.metaData;
            records = builder.records;
            setFolder();
        }


        // Set mandatory boolean of File Instance
        protected override void setFolder(){
            this.folder = false;
        }

        /// <summary>Adds a record to the list of records.</summary>
        /// <param name="record">The record to be added.</param>
        public void addRecord(Record record){
            records.Add(record);
        }


        /// <summary>Deletes a record from the list of records.</summary>
        /// <param name="record">The record to be deleted.</param>
        public void delRecord(Record record){
            records.Remove(record);
        }

        /// <summary>Deletes a record from the list of records by its ID.</summary>
        /// <param name="ID">The ID of the record to be deleted.</param>
        public void delRecordByID(int ID){
            records.RemoveAll(record => record.getRecordID() == ID);
        }

        /// <summary>Returns a list of records.</summary>
        /// <returns>A list of records.</returns>
        public List<Record> getRecords(){
            return records;
        }


        /// <summary>Retrieves a record with the specified ID.</summary>
        /// <param name="ID">The ID of the record to retrieve.</param>
        /// <returns>The record with the specified ID.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no record with the specified ID is found.</exception>
        public Record getRecord(int ID){
            foreach (Record record in records){
                if (record.getRecordID() == ID){
                    return record;
                }
            }
            return null;
        }


        // Calls EntrySubsystem
        public override void selectedAction(){
            // TODO: Call EntrySubSystem to add entry
        }


        // Builder Class for Entry
        public class EntryBuilder
        {
            public string fileName { get; set; }
            public DateTime creationDate { get; set; }
            public DateTime lastModifiedDate { get; set; }
            public string filePath { get; set; }
            public MetaData metaData { get; set; }
            public List<Record> records { get; set; }    

            EntryBuilder(MetaData metaData){
                // Set Default Values
                records = new List<Record>();
            }

            public EntryBuilder setFileName(string fileName){
                this.fileName = fileName;
                return this;
            }

            public EntryBuilder setCreationDate(DateTime creationDate){
                this.creationDate = creationDate;
                return this;
            }

            public EntryBuilder setLastModifiedDate(DateTime lastModifiedDate){
                this.lastModifiedDate = lastModifiedDate;
                return this;
            }

            public EntryBuilder setFilePath(string filePath){
                this.filePath = filePath;
                return this;
            }

            public EntryBuilder setMetaData(MetaData metaData){
                this.metaData = metaData;
                return this;
            }

            public EntryBuilder setRecords(List<Record> records){
                this.records = records;
                return this;
            }

            public Entry build(){
                return new Entry(this);
            }
        }
    }
}