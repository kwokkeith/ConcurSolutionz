using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemAcl.Decimal;

namespace ConcurSolutionz.Database
{
    public class MDBuilder
    {
        private string entryName;
        private Decimal entryBudget;


        private MDBuilder(){};


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