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
    public string SupportedLanguages { get; set; }
    public string AuctionType { get; set; }

    public DomainSearchParam()
    {
    }

    public DomainSearchParam(string vendorId, string domainName, string requestingServer, string customerIp,
        string privateLabel, string sourceCode, string visitingId, string maxDomainsPerVendor, string tlds, string supportedLanguages)
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
      SupportedLanguages = supportedLanguages;
    }

    public DomainSearchParam(string vendorId, string domainName, string requestingServer, string customerIp, string privateLabel,
        string sourceCode, string visitingId, string maxDomainsPerVendor, string tlds, string supportedLanguages, string auctionType)
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
      SupportedLanguages = supportedLanguages;
      AuctionType = auctionType;
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
          if (!string.IsNullOrEmpty(AuctionType))
          {
            writer.WriteAttributeString("auctiontype", AuctionType);
          }
          writer.WriteAttributeString("requestingserver", RequestingServer);
          writer.WriteAttributeString("customerip", CustomerIp);
          writer.WriteAttributeString("privatelabel", PrivateLabel);
          writer.WriteAttributeString("sourcecode", SourceCode);
          writer.WriteAttributeString("visitingid", VisitingId);
          writer.WriteAttributeString("maxdomainspervendor", MaxDomainsPerVendor);
          writer.WriteAttributeString("tlds", Tlds);
          writer.WriteAttributeString("supportedLanguages", SupportedLanguages);
          writer.WriteEndElement();
        }

        return sw.ToString();
      }
    }
  }
}