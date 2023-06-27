
namespace ConcurSolutionz.Database
{
    public class MDBuilder
    {
        public string entryName {get; private set;}
        public Decimal entryBudget {get; private set;}


        public MDBuilder(){}


        public MDBuilder setEntryName(string entryName){
            this.entryName = entryName;
            return this;
        }

        public MDBuilder setEntryBudget(Decimal entryBudget){
            this.entryBudget = entryBudget;
            return this;
        }
    }
}