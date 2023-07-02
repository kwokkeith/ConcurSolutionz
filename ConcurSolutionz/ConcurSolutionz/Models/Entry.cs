//using System;
//using System.Collections.Generic;
//namespace ConcurSolutionz.Models
//{
//    public class Entry
//    {
//        //MetaData and Record should be classes defined elsewhere in your code
//        private MetaData metaData;
//        private List<Record> records;

//        public Entry(EntryBuilder builder)
//        {
//            metaData = builder.MetaData;
//            records = builder.Records;
//        }

//        public void SetMetaData(MetaData metaData)
//        {
//            this.metaData = metaData;
//        }

//        public MetaData GetMetaData()
//        {
//            return metaData;
//        }

//        public void AddRecord(Record record)
//        {
//            records.Add(record);
//        }

//        public void DelRecord(Record record)
//        {
//            records.Remove(record);
//        }

//        public Record GetRecord(int ID)
//        {
//            return records.Find(record => record.ID == ID); // assuming Record class has an ID property
//        }

//        public void SelectedAction()
//        {
//            // Here, invoke the necessary method in EntrySubsystem.
//            // EntrySubsystem.YourMethod();
//        }

//        public class EntryBuilder
//        {
//            public string FileName { get; private set; }
//            public DateTime CreationDate { get; private set; }
//            public DateTime LastModifiedDate { get; private set; }
//            public string FilePath { get; private set; }
//            public MetaData MetaData { get; private set; }
//            public List<Record> Records { get; private set; }

//            public EntryBuilder()
//            {
//                Records = new List<Record>();
//            }

//            public EntryBuilder SetFileName(string name)
//            {
//                FileName = name;
//                return this;
//            }

//            public EntryBuilder SetCreationDate(DateTime date)
//            {
//                CreationDate = date;
//                return this;
//            }

//            public EntryBuilder SetLastModifiedDate(DateTime date)
//            {
//                LastModifiedDate = date;
//                return this;
//            }

//            public EntryBuilder SetFilePath(string path)
//            {
//                FilePath = path;
//                return this;
//            }

//            public EntryBuilder SetMetaData(MetaData metaData)
//            {
//                MetaData = metaData;
//                return this;
//            }

//            public EntryBuilder SetRecords(List<Record> records)
//            {
//                Records = records;
//                return this;
//            }

//            public Entry Build()
//            {
//                return new Entry(this);
//            }
//        }
//    }
//}
//// To create a new entry:
//// Entry entry = new Entry.EntryBuilder()
////.SetFileName("file_name")
////.SetCreationDate(DateTime.Now)
////.SetLastModifiedDate(DateTime.Now)
////.SetFilePath("/path/to/file")
////.SetMetaData(new MetaData())
////.SetRecords(new List<Record>())
////.Build();
using System.Collections.ObjectModel;
namespace ConcurSolutionz.Models;

internal class Entry
{
    public string Filename { get; set; }
    public ObservableCollection<Receipt> Receipts { get; set; } = new ObservableCollection<Receipt>();

}