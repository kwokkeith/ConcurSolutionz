using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurSolutionz.Database
{
    public class MDSocket
    {
        public static dynamic ConvertMetaData( MetaData metaData) {
            switch (metaData.SubType){
                case "StudentProjectClaimMetaData": {
                    return (StudentProjectClaimMetaData) metaData;
                }
                default:
                    throw new ArgumentException("Invalid MetaData subtype detected, could not convert using MDSocket!");
            }
        }
    }
}