using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WhoIs.IANA;

namespace WhoIs
{
    public class WHOISRequest
    {
        private const int WHOIS_PORT = 43;
        private const string IANA_WHOIS_SERVER = "whois.iana.org";
        private readonly string _whoisServer;

        public WHOISRequest(string whoisServer = "whois.verisign-grs.com")//IANA_WHOIS_SERVER)
        {
            _whoisServer = whoisServer;
        }

        public async Task<WHOISResponse> LookupAsync(string domain)
        {
            WHOISResponse response = new WHOISResponse();
            response.IANARecord = await IANALookupAsync(domain);
            response.WHOISRecord = await WhoIsLookupAsync(response.IANARecord.WhoIs, domain);

            return response;
        }

        private async Task<WHOISRecord> WhoIsLookupAsync(string whoisServer, string domain)
        {
            using (Socket requestSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp))
            {
                requestSocket.DualMode = true;
                requestSocket.ReceiveBufferSize = 1024;
                await requestSocket.ConnectAsync(whoisServer, WHOIS_PORT);

                string query = $"{domain}\r\n";
                byte[] queryAsBytes = Encoding.ASCII.GetBytes(query.ToCharArray());

                requestSocket.Send(queryAsBytes);

                byte[] responsePacket = new byte[1024];
                StringBuilder responseData = new StringBuilder();
                while (0 != (requestSocket.Receive(responsePacket, SocketFlags.None)))
                {
                    foreach (var @byte in responsePacket)
                    {
                        char responseContent = Convert.ToChar(@byte);
                        if (responseContent == '\r'
                            || responseContent == '\n')
                        {
                            responseData.AppendLine(" ");
                        }
                        else if (responseContent != '\0')
                        {
                            responseData.Append(responseContent.ToString());
                        }
                    }
                    
                    responsePacket = new byte[1024];
                }

                return CreateRecord(responseData.ToString().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        private async Task<IANARecord> IANALookupAsync(string domain)
        {
            using (Socket requestSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp))
            {
                requestSocket.DualMode = true;
                requestSocket.ReceiveBufferSize = 1024;
                await requestSocket.ConnectAsync(IANA_WHOIS_SERVER, WHOIS_PORT);

                string query = $"{domain}\r\n";
                byte[] queryAsBytes = Encoding.ASCII.GetBytes(query.ToCharArray());

                requestSocket.Send(queryAsBytes);

                byte[] responsePacket = new byte[1024];
                StringBuilder responseData = new StringBuilder();
                while (0 != (requestSocket.Receive(responsePacket, SocketFlags.None)))
                {
                    foreach (var @byte in responsePacket)
                    {
                        char responseContent = Convert.ToChar(@byte);
                        if (responseContent == '\r'
                            || responseContent == '\n')
                        {
                            responseData.AppendLine(" ");
                        }
                        else if (responseContent != '\0')
                        {
                            responseData.Append(responseContent.ToString());
                        }
                    }

                    responsePacket = new byte[1024];
                }
                
                return CreateIANARecord(responseData.ToString().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }
        
        public WHOISRecord CreateRecord(IEnumerable<string> content)
        {
            WHOISRecord record = new WHOISRecord();

            foreach(var line in content)
            {
                string[] data = line.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                if (data.Length == 2)
                {
                    if (data[0].Trim() == "Domain Name")
                    {
                        record.Domain = data[1].Trim();
                    }
                    else if (data[0].Trim() == "Registry Domain ID")
                    {
                        record.DomainId = data[1].Trim();
                    }
                    else if (data[0].Trim() == "Registrar WHOIS Server")
                    {
                        record.WhoIsServer = data[1].Trim();
                    }
                    else if (data[0].Trim() == "Registrar URL")
                    {
                        record.Url = data[1].Trim();
                    }
                    else if (data[0].Trim() == "Updated Date")
                    {
                        record.Updated = DateTime.Parse(data[1].Trim());
                    }
                    else if (data[0].Trim() == "Creation Date")
                    {
                        record.Created = DateTime.Parse(data[1].Trim());
                    }
                    else if (data[0].Trim() == "Registry Expiry Date")
                    {
                        record.Expiration = DateTime.Parse(data[1].Trim());
                    }
                    else if (data[0].Trim() == "Registrar")
                    {
                        record.Registrar = data[1].Trim();
                    }
                    else if (data[0].Trim() == "Registrar IANA ID")
                    {
                        record.IANAId = data[1].Trim();
                    }
                    else if (data[0].Trim() == "Registrar Abuse Contact Email")
                    {
                        record.AbuseContactEmail = data[1].Trim();
                    }
                    else if (data[0].Trim() == "Registrar Abuse Contact Phone")
                    {
                        record.AbuseContactPhone = data[1].Trim();
                    }
                    else if (data[0].Trim() == "Name Server")
                    {
                        WHOISNameServerRecord nameServer = new WHOISNameServerRecord()
                        {
                            Domain = data[1].Trim()
                        };
                        record.NameServers.Add(nameServer);
                    }
                    else if (data[0].Trim() == "DNSSEC")
                    {
                        record.DNSSec = data[1].Trim();
                    }
                }
            }

            return record;
        }

        private IANARecord CreateIANARecord(IEnumerable<string> content)
        {
            IANARecord record = new IANARecord();
            IANAContactRecord contact = new IANAContactRecord();
            int addressCount = 1;
            int organisationCount = 1;
            foreach (var line in content)
            {
                string[] data = line.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                if (data.Length == 2)
                {
                    if (data[0] == "refer")
                    {
                        record.Refer = data[1].Trim();
                    }
                    else if (data[0] == "domain")
                    {
                        record.Domain = data[1].Trim();
                    }
                    else if (data[0] == "organisation" && organisationCount == 1)
                    {
                        record.Organisation = data[1].Trim();
                        organisationCount++;
                    }
                    else if (data[0] == "address" && addressCount == 1)
                    {
                        record.Address1 = data[1].Trim();
                        addressCount++;
                    }
                    else if (data[0] == "address" && addressCount == 2)
                    {
                        record.Address2 = data[1].Trim();
                        addressCount++;
                    }
                    else if (data[0] == "address" && addressCount == 3)
                    {
                        record.Address3 = data[1].Trim();
                        addressCount++;
                    }
                    else if (data[0] == "contact")
                    {
                        contact = new IANAContactRecord()
                        {
                            Contact = data[1].Trim()
                        };
                        record.Contacts.Add(contact);
                    }
                    else if (data[0] == "name")
                    {
                        contact.Name = data[1].Trim();
                    }
                    else if (data[0] == "orgnisation" && organisationCount > 1)
                    {
                        contact.Organisation = data[1].Trim();
                    }
                    else if (data[0] == "address" && (addressCount % 3) == 1)
                    {
                        contact.Address1 = data[1].Trim();
                        addressCount++;
                    }
                    else if (data[0] == "address" && (addressCount % 3) == 2)
                    {
                        contact.Address2 = data[1].Trim();
                        addressCount++;
                    }
                    else if (data[0] == "address" && (addressCount % 3) == 3)
                    {
                        contact.Address3 = data[1].Trim();
                        addressCount = 4;
                    }
                    else if (data[0] == "phone")
                    {
                        contact.Phone = data[1].Trim();
                    }
                    else if (data[0] == "fax-no")
                    {
                        contact.Fax = data[1].Trim();
                    }
                    else if (data[0] == "e-mail")
                    {
                        contact.Email = data[1].Trim();
                    }
                    else if (data[0] == "nserver")
                    {
                        IANANameServerRecord nameServer = new IANANameServerRecord()
                        {
                            Domain = data[1].Trim()
                        };
                        record.NameServers.Add(nameServer);
                    }
                    else if (data[0] == "ds-rdata")
                    {
                        record.DSRData = data[1].Trim();
                    }
                    else if (data[0] == "whois")
                    {
                        record.WhoIs = data[1].Trim();
                    }
                    else if (data[0] == "status")
                    {
                        record.Status = data[1].Trim();
                    }
                    else if (data[0] == "remarks")
                    {
                        record.Remarks = data[1].Trim();
                    }
                    else if (data[0] == "created")
                    {
                        record.Created = DateTime.Parse(data[1].Trim());
                    }
                    else if (data[0] == "changed")
                    {
                        record.Changed = DateTime.Parse(data[1].Trim());
                    }
                    else if (data[0] == "source")
                    {
                        record.Source = data[1].Trim();
                    }
                }
            }

            return record;
        }
    }
}
