using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIs.IANA
{
    public class IANAContactRecord
    {
        public string Contact { get; set; }
        public string Name { get; set; }
        public string Organisation { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
    }
}
