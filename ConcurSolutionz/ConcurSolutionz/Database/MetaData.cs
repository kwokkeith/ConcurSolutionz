
namespace ConcurSolutionz.Database
{
    public class MetaData
    {
        public string EntryName {get; set;}
        private decimal entryBudget;
        public decimal EntryBudget { get
            {
                return entryBudget;
            }
            set
            {
                Utilities.CheckIfNegative(value);
                entryBudget = value;
            }
        }
        public string SubType { get; set; }
    }
}