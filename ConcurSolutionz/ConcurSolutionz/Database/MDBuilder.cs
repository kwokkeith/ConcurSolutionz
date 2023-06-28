
namespace ConcurSolutionz.Database
{
    public class MDBuilder
    {
        public string EntryName {get; private set;}
        public decimal EntryBudget {get; private set;}


        public MDBuilder(){}


        public MDBuilder SetEntryName(string entryName){
            this.EntryName = entryName;
            return this;
        }

        public MDBuilder SetEntryBudget(decimal entryBudget){
            this.EntryBudget = entryBudget;
            return this;
        }
    }
}