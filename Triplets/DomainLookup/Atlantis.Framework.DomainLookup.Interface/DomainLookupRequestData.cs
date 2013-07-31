using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainLookup.Interface
{
    public class DomainLookupRequestData : RequestData
    {
        /// <summary>
        /// Domain name that you need to retrieve information for (owning shopper id, status id from domain info, 60 day lock status, etc.)
        /// </summary>
        private string _domainName;

        public DomainLookupRequestData(string domainName)
        {
            _domainName = domainName;
        }

        public override string ToXML()
        {
            StringBuilder sbRequest = new StringBuilder();
            XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

            xtwRequest.WriteStartElement("DomainLookupRequest");
            xtwRequest.WriteStartElement("DomainName");
            xtwRequest.WriteCData(_domainName);
            xtwRequest.WriteEndElement();
            xtwRequest.WriteEndElement(); 
            return sbRequest.ToString();
        }

        /// <summary>
        /// Domain name that you need to retrieve information for (owning shopper id, status id from domain info, 60 day lock status, etc.)
        /// </summary>
        public string DomainName
        {
            get { return _domainName; }
            set { _domainName = value; }
        }
    }
}
