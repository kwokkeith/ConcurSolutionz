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
        public string policy {get; private set;}
        public string claimName {get; private set;}
        public DateTime claimDate {get; private set;}
        public string purpose {get; private set;}
        public string teamName {get; private set;}
        public string projectClub {get; private set;}
        
        private ClaimMDBuilder(){
        }

        public ClaimMDBuilder setPolicy(string policy){
            this.policy = policy;
            return this;
        }

        public ClaimMDBuilder setClaimName(string claimName){
            this.claimName = claimName;
            return this;
        }

        public ClaimMDBuilder setClaimDate(DateTime claimDate){
            this.claimDate = claimDate;
            return this;
        }

        public ClaimMDBuilder setPurpose(string purpose){
            this.purpose = purpose;
            return this;
        }

        public ClaimMDBuilder setTeamName(string teamName){
            this.teamName = teamName;
            return this;
        }

        public ClaimMDBuilder setProjectClub(string projectClubName){
            this.projectClub = projectClubName;
            return this;
        }

        protected ClaimMetaData build(){
            return new ClaimMetaData(this);
        }
    }
}