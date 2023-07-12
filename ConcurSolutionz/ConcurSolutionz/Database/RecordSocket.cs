using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurSolutionz.Database
{
    public class RecordSocket
    {
        public static dynamic ConvertRecord( Record record) {
            switch (record.SubType){
                case "Receipt": 
                    return (Receipt) record;
                default:
                    throw new ArgumentException("Invalid Record subtype detected, could not convert using RecordSocket!");
            }
        }
    }
}