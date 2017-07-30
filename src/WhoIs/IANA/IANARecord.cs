using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIs.IANA
{
    public class IANARecord
    {
		public string Refer { get; set; }
		public string Domain { get; set; }
		public string Organisation { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Address3 { get; set; }

        public IList<IANAContactRecord> Contacts { get; set; } = new List<IANAContactRecord>();
        public IList<IANANameServerRecord> NameServers { get; set; } = new List<IANANameServerRecord>();
		public string DSRData { get; set; }
		public string WhoIs { get; set; }
		public string Status { get; set; }
		public string Remarks { get; set; }
		public DateTime Created { get; set; }
		public DateTime Changed { get; set; }
		public string Source { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Refer: {Refer}");
            stringBuilder.AppendLine($"Domain: {Domain}");
            stringBuilder.AppendLine($"Organisation: {Organisation}");
            stringBuilder.AppendLine($"Address: {Address1}");
            stringBuilder.AppendLine($"Address: {Address2}");
            stringBuilder.AppendLine($"Address: {Address3}");

            foreach (var contact in Contacts)
            {
                stringBuilder.AppendLine($"{Environment.NewLine}Contact Information{Environment.NewLine}");
                stringBuilder.AppendLine($"\tContact: {contact.Contact}");
                stringBuilder.AppendLine($"\tName: {contact.Name}");
                stringBuilder.AppendLine($"\tOrganisation: {Organisation}");
                stringBuilder.AppendLine($"\tAddress: {Address1}");
                stringBuilder.AppendLine($"\tAddress: {Address2}");
                stringBuilder.AppendLine($"\tAddress: {Address3}");
                stringBuilder.AppendLine($"\tPhone: {contact.Phone}");
                stringBuilder.AppendLine($"\tFax: {contact.Fax}");
                stringBuilder.AppendLine($"\tEmail: {contact.Email}");
            }

            stringBuilder.AppendLine($"{Environment.NewLine}Name Servers{Environment.NewLine}");
            foreach (var nameServer in NameServers)
            {
                stringBuilder.AppendLine($"\tDomain: {nameServer.Domain}");
            }

            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine($"DS-RData: {DSRData}");
            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine($"WhoIs: {WhoIs}");
            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine($"Status: {Status}");
            stringBuilder.AppendLine($"Remarks: {Remarks}");
            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine($"Created: {Created.ToString()}");
            stringBuilder.AppendLine($"Changed: {Changed.ToString()}");
            stringBuilder.AppendLine($"Source: {Source}");


            return stringBuilder.ToString();
        }
    }
}
