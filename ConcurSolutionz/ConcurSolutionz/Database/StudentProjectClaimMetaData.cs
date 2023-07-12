
namespace ConcurSolutionz.Database
{
    public class StudentProjectClaimMetaData : MetaData
    {
        public string Policy { get; set; } 
        public string ClaimName { get; set; }
        private DateTime claimDate;
        public DateTime ClaimDate {
            get{ return claimDate; }
            set {
                Utilities.CheckDateTimeAheadOfNow(value);
                claimDate = value;
            }
        }
        public string Purpose { get; set; }
        public string TeamName { get; set; }
        public string ProjectClub { get; set; }

        public StudentProjectClaimMetaData(StudentProjectClaimMDBuilder claimMDBuilder){
            // Check if attributes have been declared (Mandatory)
            Utilities.CheckNull(claimMDBuilder.EntryName);
            Utilities.CheckNull(claimMDBuilder.EntryBudget);
            Utilities.CheckNull(claimMDBuilder.Policy);
            Utilities.CheckNull(claimMDBuilder.ClaimName);
            Utilities.CheckNull(claimMDBuilder.ClaimDate);
            Utilities.CheckNull(claimMDBuilder.Purpose);
            Utilities.CheckNull(claimMDBuilder.TeamName);
            Utilities.CheckNull(claimMDBuilder.ProjectClub);
            Utilities.CheckNull(claimMDBuilder.SubType);

            // If all variables are declared then create ClaimMetaData
            this.EntryName = claimMDBuilder.EntryName;
            this.EntryBudget = claimMDBuilder.EntryBudget;
            this.Policy = claimMDBuilder.Policy;
            this.ClaimName = claimMDBuilder.ClaimName;
            this.ClaimDate = claimMDBuilder.ClaimDate;
            this.Purpose = claimMDBuilder.Purpose;
            this.TeamName = claimMDBuilder.TeamName;
            this.ProjectClub = claimMDBuilder.ProjectClub;
            this.SubType = claimMDBuilder.SubType;
        }
    }
}