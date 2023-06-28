
namespace ConcurSolutionz.Database
{
    public abstract class Record
    {
        protected int RecordID;

        abstract public void AssignRecordID();  
        // Calculate RecordID later
        abstract public int GetRecordID();
        abstract public void DelRecord();   
    }
}