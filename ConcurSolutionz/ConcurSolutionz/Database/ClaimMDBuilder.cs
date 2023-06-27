using System.Web;
using System.Net;
using System.Xml.Xsl;
using System.Security.Claims;
using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurSolutionz.Database
{
    public class ClaimMDBuilder :  MDBuilder
    {
        private string policy;
        private string claimName;
        private string claimDate;
        private string purpose;
        private string teamName;
        private string projectClub;
        
        public ClaimMDBuilder(){
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

        public ClaimMetaData build(){
            return ClaimMetaData(this);
        }
    }
}