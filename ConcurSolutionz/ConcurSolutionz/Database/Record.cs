
namespace ConcurSolutionz.Database
{
    public abstract class Record
    {
        protected int RecordID;

        public string ImgPath { get; set; }

        abstract public void AssignRecordID();  
        // Calculate RecordID later
        abstract public int GetRecordID();
        abstract public void DelRecord();   
    }
}