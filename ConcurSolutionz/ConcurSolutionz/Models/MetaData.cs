//using System;
//namespace ConcurSolutionz.Models
//{
//	public interface MetaData
//	{
//        string EntryName { get; set; }
//        decimal EntryBudget { get; set; }

//        void SetEntryName(string entryName);
//        void SetEntryBudget(decimal budget);
//        string GetEntryName();
//        decimal GetEntryBudget();
//    }
//    public class MDBuilder
//    {
//        public string EntryName { get; set; }
//        public decimal EntryBudget { get; set; }

//        public MDBuilder SetEntryName(string entryName)
//        {
//            EntryName = entryName;
//            return this;
//        }

//        public MDBuilder SetEntryBudget(decimal budget)
//        {
//            EntryBudget = budget;
//            return this;
//        }
//    }

//    public class ClaimMetaData : MetaData
//    {
//        public string EntryName { get; set; }
//        public decimal EntryBudget { get; set; }
//        public string Policy { get; set; }

//        public ClaimMetaData(ClaimMDBuilder builder)
//        {
//            EntryName = builder.EntryName;
//            EntryBudget = builder.EntryBudget;
//            Policy = builder.Policy;
//        }

//        public void SetEntryName(string entryName)
//        {
//            EntryName = entryName;
//        }

//        public void SetEntryBudget(decimal budget)
//        {
//            EntryBudget = budget;
//        }

//        public string GetEntryName()
//        {
//            return EntryName;
//        }

//        public decimal GetEntryBudget()
//        {
//            return EntryBudget;
//        }
//    }

//    public class ClaimMDBuilder : MDBuilder
//    {
//        public string Policy { get; set; }

//        public ClaimMDBuilder() { }

//        public new ClaimMDBuilder SetEntryName(string entryName)
//        {
//            EntryName = entryName;
//            return this;
//        }

//        public new ClaimMDBuilder SetEntryBudget(decimal budget)
//        {
//            EntryBudget = budget;
//            return this;
//        }

//        public ClaimMDBuilder SetPolicy(string policy)
//        {
//            Policy = policy;
//            return this;
//        }
//    }

//    public class RequestMDBuilder : MDBuilder
//    {
//        public string Policy { get; set; }
//        public string ClaimName { get; set; }
//        public DateTime ClaimDate { get; set; }
//        public string Purpose { get; set; }
//        public string TeamName { get; set; }
//        public string ProjectClub { get; set; }

//        public RequestMDBuilder() { }

//        public new RequestMDBuilder SetEntryName(string entryName)
//        {
//            EntryName = entryName;
//            return this;
//        }

//        public new RequestMDBuilder SetEntryBudget(decimal budget)
//        {
//            EntryBudget = budget;
//            return this;
//        }

//        public RequestMDBuilder SetPolicy(string policy)
//        {
//            Policy = policy;
//            return this;
//        }

//        public RequestMDBuilder SetClaimName(string claimName)
//        {
//            ClaimName = claimName;
//            return this;
//        }

//        public RequestMDBuilder SetClaimDate(DateTime date)
//        {
//            ClaimDate = date;
//            return this;
//        }

//        public RequestMDBuilder SetPurpose(string purpose)
//        {
//            Purpose = purpose;
//            return this;
//        }

//        public RequestMDBuilder SetTeamName(string teamName)
//        {
//            TeamName = teamName;
//            return this;
//        }

//        public RequestMDBuilder SetProjectClub(string projectClubName)
//        {
//            ProjectClub = projectClubName;
//            return this;
//        }

//        public RequestMetaData Build()
//        {
//            return new RequestMetaData(this);
//        }
//    }

//    public class RequestMetaData : MetaData
//    {
//        public string EntryName { get; set; }
//        public decimal EntryBudget { get; set; }
//        public string Policy { get; set; }
//        public string ClaimName { get; set; }
//        public DateTime ClaimDate { get; set; }
//        public string Purpose { get; set; }
//        public string TeamName { get; set; }
//        public string ProjectClub { get; set; }

//        public RequestMetaData(RequestMDBuilder builder)
//        {
//            EntryName = builder.EntryName;
//            EntryBudget = builder.EntryBudget;
//            Policy = builder.Policy;
//            ClaimName = builder.ClaimName;
//            ClaimDate = builder.ClaimDate;
//            Purpose = builder.Purpose;
//            TeamName = builder.TeamName;
//            ProjectClub = builder.ProjectClub;
//        }

//        public void SetEntryName(string entryName)
//        {
//            EntryName = entryName;
//        }

//        public void SetEntryBudget(decimal budget)
//        {
//            EntryBudget = budget;
//        }

//        public string GetEntryName()
//        {
//            return EntryName;
//        }

//        public decimal GetEntryBudget()
//        {
//            return EntryBudget;
//        }
//    }
//}

