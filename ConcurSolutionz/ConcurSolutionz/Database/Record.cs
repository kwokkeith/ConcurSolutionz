
namespace ConcurSolutionz.Database
{
    public abstract class Record
    {
        public int RecordID {get; set; }

        public string RecordSubclass {get; set;}

        abstract protected void AddSubClassRecord();
    }
}