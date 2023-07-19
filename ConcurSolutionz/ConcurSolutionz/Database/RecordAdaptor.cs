using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurSolutionz.Database
{
    public class RecordAdaptor
    {
        public static dynamic ConvertRecord( Record record) {
            if (record.SubType == typeof(Receipt).FullName)
            {
                return (Receipt)record;
            }
            else
            { 
                throw new ArgumentException("Invalid Record subtype detected, could not convert using RecordSocket!");
            }
        }
    }
}