
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
            Utilities.checkNull(claimMDBuilder.entryName);
            Utilities.checkNull(claimMDBuilder.entryBudget);
            Utilities.checkNull(claimMDBuilder.policy);
            Utilities.checkNull(claimMDBuilder.claimName);
            Utilities.checkNull(claimMDBuilder.claimDate);
            Utilities.checkNull(claimMDBuilder.purpose);
            Utilities.checkNull(claimMDBuilder.teamName);
            Utilities.checkNull(claimMDBuilder.projectClub);

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