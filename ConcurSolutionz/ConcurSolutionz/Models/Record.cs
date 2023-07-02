<<<<<<< HEAD
﻿using System;
namespace ConcurSolutionz
{
    public interface Record
    {
        int RecordId { get; }
        void AssignRecordId();
        int GetRecordId();
        void DelRecord();
    }
}
=======
﻿namespace ConcurSolutionz.Models;

internal class Record
{
    public string RecordName { get; set; }
    public string CreationDate { get; set; }
    public double Amount { get; set; }
}

>>>>>>> record_page
