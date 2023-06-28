
namespace ConcurSolutionz.Database
{
    public abstract class Record
    {
        public int RecordID {get; set; }

        abstract public void DelRecord();   
    }
}