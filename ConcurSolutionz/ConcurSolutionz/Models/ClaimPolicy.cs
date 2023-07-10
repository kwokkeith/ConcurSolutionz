﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurSolutionz.Models
{
    //Object to deserialize the different possible claim policies
    public class ClaimPolicy
    {
        public string code { get; set; }
        public string id { get; set; }
        public string matchValue { get; set; }
        public string serviceVersion { get; set; }
        public string shortCode {get; set;}
        public string value { get; set; }
        public string __typename { get; set; }

    }
}
