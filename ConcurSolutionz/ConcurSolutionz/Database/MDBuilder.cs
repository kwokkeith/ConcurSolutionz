
namespace ConcurSolutionz.Database
{
    public abstract class MDBuilder
    {
        public string EntryName {get; protected set;}
        public decimal EntryBudget {get; protected set;}
        public const string DEFAULT_ENTRYNAME = "Unnamed";
        public const decimal DEFAULT_BUDGET = 0m;

        public MDBuilder(){}


        public abstract MDBuilder SetEntryName(string entryName);
        public abstract MDBuilder SetEntryBudget(decimal entryBudget);
    }
}