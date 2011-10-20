using System.IO;
using System.Xml;

namespace Atlantis.Framework.RegVendorDomainSearch.Interface
{
    public class DomainSearchParam
    {
        public string VendorId { get; set; }
        public string DomainName { get; set; }
        public string RequestingServer { get; set; }
        public string CustomerIp { get; set; }
        public string PrivateLabel { get; set; }
        public string SourceCode { get; set; }
        public string VisitingId { get; set; }
        public string MaxDomainsPerVendor { get; set; }
        public string Tlds { get; set; }

        public DomainSearchParam()
        {

        }

        public DomainSearchParam(string vendorId, string domainName, string requestingServer, string customerIp,
            string privateLabel, string sourceCode, string visitingId, string maxDomainsPerVendor, string tlds)
        {
            VendorId = vendorId;
            DomainName = domainName;
            RequestingServer = requestingServer;
            CustomerIp = customerIp;
            PrivateLabel = privateLabel;
            SourceCode = sourceCode;
            VisitingId = visitingId;
            MaxDomainsPerVendor = maxDomainsPerVendor;
            Tlds = tlds;
        }

        public string ToXml()
        {
          using (StringWriter sw = new StringWriter())
            {
              using (XmlTextWriter writer = new XmlTextWriter(sw))
                {
                    writer.WriteStartElement("dppdomainsearch");
                    writer.WriteAttributeString("vendorid", VendorId);
                    writer.WriteAttributeString("domainname", DomainName);
                    writer.WriteAttributeString("requestingserver", RequestingServer);
                    writer.WriteAttributeString("customerip", CustomerIp);
                    writer.WriteAttributeString("privatelabel", PrivateLabel);
                    writer.WriteAttributeString("sourcecode", SourceCode);
                    writer.WriteAttributeString("visitingid", VisitingId);
                    writer.WriteAttributeString("maxdomainspervendor", MaxDomainsPerVendor);
                    writer.WriteAttributeString("tlds", Tlds);
                    writer.WriteEndElement();
                }

                return sw.ToString();
            }
        }
    }
}
