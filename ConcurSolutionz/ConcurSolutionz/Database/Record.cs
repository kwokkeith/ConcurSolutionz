
namespace ConcurSolutionz.Database
{
    public abstract class Record
    {
        protected int recordID;
        
        abstract public void assignRecordID();  
        // Calculate RecordID later
        abstract public int getRecordID();
        abstract public void delRecord();   
    }
}