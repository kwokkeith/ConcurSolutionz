
namespace ConcurSolutionz.Database
{
    public abstract class Record
    {
        protected int recordID;
        
        abstract public void AssignRecordID();  
        // Calculate RecordID later
        abstract public int GetRecordID();
        abstract public void DelRecord();   
    }
}