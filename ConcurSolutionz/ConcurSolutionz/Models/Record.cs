using System;
namespace ConcurSolutionz
{
    public interface Record
    {
        int RecordId { get; }
        void AssignRecordId();
        int GetRecordId();
        void DelRecord();
    }
}