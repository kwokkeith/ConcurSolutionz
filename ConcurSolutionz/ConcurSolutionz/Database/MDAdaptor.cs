using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurSolutionz.Database
{
    public class MDAdaptor
    {
        public static dynamic ConvertMetaData(MetaData metaData) {
            if (metaData.SubType == typeof(StudentProjectClaimMetaData).FullName)
            { 
              return (StudentProjectClaimMetaData) metaData;
            }
            else
            { 
                throw new ArgumentException("Invalid MetaData subtype detected, could not convert using MDSocket!");
            }
        }
    }
}