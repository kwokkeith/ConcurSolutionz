using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurSolutionz.Database
{
    public abstract class Record
    {
        protected int recordID;
        
        abstract public void assignRecordID();  
        // Calculate RecordID later
        abstract public int getRecordID();
        abstract public void delRecord();   
    }
}