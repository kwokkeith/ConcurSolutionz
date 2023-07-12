using System.Web;
using System.Net;
using System.Xml.Xsl;
using System.Security.Claims;
using System.Security.AccessControl;
using System;
using System.Collections.Generic;

namespace ConcurSolutionz.Database
{
    public class StudentProjectClaimMDBuilder :  MDBuilder
    {
        public string Policy {get; private set;}
        public string ClaimName {get; private set;}
        public DateTime ClaimDate {get; private set;}
        public string Purpose {get; private set;}
        public string TeamName {get; private set;}
        public string ProjectClub {get; private set;}
        public string SubType { get; private set; }
        private const string POLICY = "Student Project Claim";
        private const string SUBTYPE = "StudentProjectClaimMetaData";


        public StudentProjectClaimMDBuilder(){
            Policy = POLICY;
            SubType = SUBTYPE;
            EntryBudget = DEFAULT_BUDGET;
            EntryName = DEFAULT_ENTRYNAME;
        }

        public StudentProjectClaimMDBuilder SetClaimName(string ClaimName){
            this.ClaimName = ClaimName;
            return this;
        }

        public StudentProjectClaimMDBuilder SetClaimDate(DateTime ClaimDate){
            Utilities.CheckDateTimeAheadOfNow(ClaimDate);
            this.ClaimDate = ClaimDate;
            return this;
        }

        public StudentProjectClaimMDBuilder SetPurpose(string Purpose){
            this.Purpose = Purpose;
            return this;
        }

        public StudentProjectClaimMDBuilder SetTeamName(string TeamName){
            this.TeamName = TeamName;
            return this;
        }

        public StudentProjectClaimMDBuilder SetProjectClub(string ProjectClubName){
            this.ProjectClub = ProjectClubName;
            return this;
        }

        public override StudentProjectClaimMDBuilder SetEntryName(string EntryName){ 
            this.EntryName = EntryName;
            return this;
        }

        public override StudentProjectClaimMDBuilder SetEntryBudget(decimal EntryBudget) {
            Utilities.CheckIfNegative(EntryBudget);
            this.EntryBudget = EntryBudget;
            return this;
        }

            public StudentProjectClaimMetaData Build(){
            return new StudentProjectClaimMetaData(this);
        }
    }
}