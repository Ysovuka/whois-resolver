using System;
using System.Collections.Generic;
using System.Text;
using WhoIs.IANA;

namespace WhoIs
{
    public class WHOISResponse
    {
        public IANARecord IANARecord { get; set; }
        public WHOISRecord WHOISRecord { get; set; }
    }
}
