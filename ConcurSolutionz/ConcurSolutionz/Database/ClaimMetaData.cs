
namespace ConcurSolutionz.Database
{
    public class ClaimMetaData : MetaData
    {
        public string Policy { get; set; } 
        public string ClaimName { get; set; }
        public DateTime ClaimDate { get; set; }
        public string Purpose { get; set; }
        public string TeamName { get; set; }
        public string ProjectClub { get; set; }

        public ClaimMetaData(ClaimMDBuilder claimMDBuilder){
            // Check if attributes have been declared (Mandatory)
            Utilities.CheckNull(claimMDBuilder.entryName);
            Utilities.CheckNull(claimMDBuilder.entryBudget);
            Utilities.CheckNull(claimMDBuilder.policy);
            Utilities.CheckNull(claimMDBuilder.claimName);
            Utilities.CheckNull(claimMDBuilder.claimDate);
            Utilities.CheckNull(claimMDBuilder.purpose);
            Utilities.CheckNull(claimMDBuilder.teamName);
            Utilities.CheckNull(claimMDBuilder.projectClub);

            // If all variables are declared then create ClaimMetaData
            this.entryName = claimMDBuilder.entryName;
            this.entryBudget = claimMDBuilder.entryBudget;
            this.Policy = claimMDBuilder.policy;
            this.ClaimName = claimMDBuilder.claimName;
            this.ClaimDate = claimMDBuilder.claimDate;
            this.Purpose = claimMDBuilder.purpose;
            this.TeamName = claimMDBuilder.teamName;
            this.ProjectClub = claimMDBuilder.projectClub;
        }
    }
}