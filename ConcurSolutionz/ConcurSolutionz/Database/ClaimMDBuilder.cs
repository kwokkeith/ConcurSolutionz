using System.Web;
using System.Net;
using System.Xml.Xsl;
using System.Security.Claims;
using System.Security.AccessControl;
using System;
using System.Collections.Generic;

namespace ConcurSolutionz.Database
{
    public class ClaimMDBuilder :  MDBuilder
    {
        public string Policy {get; private set;}
        public string ClaimName {get; private set;}
        public DateTime ClaimDate {get; private set;}
        public string Purpose {get; private set;}
        public string TeamName {get; private set;}
        public string ProjectClub {get; private set;}
        
        private ClaimMDBuilder(){
        }

        public ClaimMDBuilder SetPolicy(string Policy){
            this.Policy = Policy;
            return this;
        }

        public ClaimMDBuilder SetClaimName(string ClaimName){
            this.ClaimName = ClaimName;
            return this;
        }

        public ClaimMDBuilder SetClaimDate(DateTime ClaimDate){
            this.ClaimDate = ClaimDate;
            return this;
        }

        public ClaimMDBuilder SetPurpose(string Purpose){
            this.Purpose = Purpose;
            return this;
        }

        public ClaimMDBuilder SetTeamName(string TeamName){
            this.TeamName = TeamName;
            return this;
        }

        public ClaimMDBuilder SetProjectClub(string ProjectClubName){
            this.ProjectClub = ProjectClubName;
            return this;
        }

        protected ClaimMetaData Build(){
            return new ClaimMetaData(this);
        }
    }
}