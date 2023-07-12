//using System;
//namespace ConcurSolutionz
//{
//    public interface Record
//    {
//        int RecordId { get; }
//        void AssignRecordId();
//        int GetRecordId();
//        void DelRecord();
//    }
//}

namespace ConcurSolutionz.Database
{
    public abstract class Record
    {
        public int RecordID { get; set; }

        public string SubType { get; set; }

    }
}