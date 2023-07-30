
using System.Text.Json.Serialization;
using System.Text.Json;

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

            // If all variables are declared then create ClaimMetaData
            this.EntryName = claimMDBuilder.EntryName;
            this.EntryBudget = claimMDBuilder.EntryBudget;
            this.Policy = claimMDBuilder.Policy;
            this.ClaimName = claimMDBuilder.ClaimName;
            this.ClaimDate = claimMDBuilder.ClaimDate;
            this.Purpose = claimMDBuilder.Purpose;
            this.TeamName = claimMDBuilder.TeamName;
            this.ProjectClub = claimMDBuilder.ProjectClub;
            this.SubType = GetType().FullName;
        }

        public class StudentProjectClaimMetaDataConverter : JsonConverter<StudentProjectClaimMetaData>
        {
            public override void Write(Utf8JsonWriter writer, StudentProjectClaimMetaData value, JsonSerializerOptions options)
            {
                // Write JSON
            }

            public override StudentProjectClaimMetaData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // Manually override parsing of JSON and return a StudentProjectClaimMetaData object
                JsonDocument doc = JsonDocument.ParseValue(ref reader);
                string entryName = doc.RootElement.GetProperty("EntryName").GetString();
                decimal entryBudget = doc.RootElement.GetProperty("EntryBudget").GetDecimal();
                string policy = doc.RootElement.GetProperty("Policy").GetString();
                string claimName = doc.RootElement.GetProperty("ClaimName").GetString();
                DateTime claimDate = doc.RootElement.GetProperty("ClaimDate").GetDateTime();
                string purpose = doc.RootElement.GetProperty("Purpose").GetString();
                string teamName = doc.RootElement.GetProperty("TeamName").GetString();
                string projectClub = doc.RootElement.GetProperty("ProjectClub").GetString();
                string subType = doc.RootElement.GetProperty("SubType").GetString();


                StudentProjectClaimMDBuilder builder = new StudentProjectClaimMDBuilder();

                StudentProjectClaimMetaData md = builder.SetClaimName(claimName)
                                                        .SetClaimDate(claimDate)
                                                        .SetPurpose(purpose)
                                                        .SetTeamName(teamName)
                                                        .SetProjectClub(projectClub)
                                                        .SetEntryName(entryName)
                                                        .SetEntryBudget(entryBudget)
                                                        .Build();

                return md;
            }
        }
    }
}