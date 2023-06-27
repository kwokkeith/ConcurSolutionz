using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurSolutionz.Database
{
    public class ClaimMetaData : MetaData
    {
        private string policy { get; set; } 
        private string claimName { get; set; }
        private DateTime claimDate { get; set; }
        private string purpose { get; set; }
        private string teamName { get; set; }
        private string projectClub { get; set; }

        public ClaimMetaData(ClaimMDBuilder claimMDBuilder){
            // Check if attributes have been declared (Mandatory)
            CustomUtility.checkNull(claimMDBuilder.entryName);
            CustomUtility.checkNull(ClaimMDBuilder.entryBudget);
            CustomUtility.checkNull(claimMDBuilder.policy);
            CustomUtility.checkNull(claimMDBuilder.claimName);
            CustomUtility.checkNull(claimMDBuilder.claimDate);
            CustomUtility.checkNull(claimMDBuilder.purpose);
            CustomUtility.checkNull(claimMDBuilder.teamName);
            CustomUtility.checkNull(claimMDBuilder.projectClub);

            // If all variables are declared then create ClaimMetaData
            this.entryName = claimMDBuilder.entryName;
            this.entryBudget = claimMDBuilder.entryBudget;
            this.policy = claimMDBuilder.policy;
            this.claimName = claimMDBuilder.claimName;
            this.claimDate = claimMDBuilder.claimDate;
            this.purpose = claimMDBuilder.purpose;
            this.teamName = claimMDBuilder.teamName;
            this.projectClub = claimMDBuilder.projectClub;
        }
    }
}