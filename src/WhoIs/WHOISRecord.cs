using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIs
{
    public class WHOISRecord
    {
        public string Domain { get; set; }
        public string DomainId { get; set; }
        public string WhoIsServer { get; set; }
        public string Url { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expiration { get; set; }
        public string IANAId { get; set; }
        public string Registrar { get; set; }
        public IList<WHOISNameServerRecord> NameServers { get; set; } = new List<WHOISNameServerRecord>();
        public string DNSSec { get; set; }
        public string AbuseContactEmail { get; set; }
        public string AbuseContactPhone { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Domain: {Domain}");
            stringBuilder.AppendLine($"Registry Domain Id: {DomainId}");
            stringBuilder.AppendLine($"Registrar WhoIs Server: {WhoIsServer}");
            stringBuilder.AppendLine($"Registrar Url: {Url}");
            stringBuilder.AppendLine($"Updated Date: {Updated.ToString()}");
            stringBuilder.AppendLine($"Creation Date: {Created.ToString()}");
            stringBuilder.AppendLine($"Expiration Date: {Expiration.ToString()}");
            stringBuilder.AppendLine($"Registrar: {Registrar}");
            stringBuilder.AppendLine($"Registrar IANA Id: {IANAId}");
            stringBuilder.AppendLine($"Registrar Abuse Contact Email: {AbuseContactEmail}");
            stringBuilder.AppendLine($"Registrar Abuse Contact Phone: {AbuseContactPhone}");

            stringBuilder.AppendLine($"{Environment.NewLine}Name Servers{Environment.NewLine}");
            foreach (var nameServer in NameServers)
            {
                stringBuilder.AppendLine($"\tDomain: {nameServer.Domain}");
            }

            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine($"DNSSec: {DNSSec}");


            return stringBuilder.ToString();
        }
    }
}
