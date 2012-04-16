using System.IO;
using System.Xml;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.RegVendorDomainSearch.Interface
{
  public class RegVendorDomainSearchRequestData : RequestData
  {
    private RegVendorDomainSearchVendorList _vendorList;
    private string _domainName;
    private string _requestingServer;
    private string _customerIp;
    private int _privateLabelID;
    private string _sourceCode;
    private string _visitingId;
    private int _maxDomainsPerVendor;
    private string _tlds;
    private string _searchDatabase = string.Empty;
    private string _supportedLanguages = string.Empty;

    public TimeSpan _requestTimeout = TimeSpan.FromMilliseconds(2500);

    [Obsolete("This constructor is obsolete. Use constructor with (string searchDatabase) instead of one with (RegVendorDomainSearchVendorList vendorList).")]
    public RegVendorDomainSearchRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount,
        RegVendorDomainSearchVendorList vendorList, string domainName, string requestingServer, string customerIp,
        int privateLabelID, string sourceCode, int maxDomainsPerVendor, string tlds)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _vendorList = vendorList;
      _domainName = domainName;
      _requestingServer = requestingServer;
      _customerIp = customerIp;
      _privateLabelID = privateLabelID;
      _sourceCode = sourceCode;
      _visitingId = pathway;
      _maxDomainsPerVendor = maxDomainsPerVendor;
      _tlds = tlds;
    }

    public RegVendorDomainSearchRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount,
        string searchDatabase, string domainName, string requestingServer, string customerIp,
        int privateLabelID, string sourceCode, int maxDomainsPerVendor, string tlds)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      this._searchDatabase = searchDatabase;
      this._domainName = domainName;
      this._requestingServer = requestingServer;
      this._customerIp = customerIp;
      this._privateLabelID = privateLabelID;
      this._sourceCode = sourceCode;
      this._visitingId = pathway;
      this._maxDomainsPerVendor = maxDomainsPerVendor;
      this._tlds = tlds;
    }

    public RegVendorDomainSearchRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount,
        string searchDatabase, string domainName, string requestingServer, string customerIp,
        int privateLabelID, string sourceCode, int maxDomainsPerVendor, string tlds, string supportedLanguages)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      this._searchDatabase = searchDatabase;
      this._domainName = domainName;
      this._requestingServer = requestingServer;
      this._customerIp = customerIp;
      this._privateLabelID = privateLabelID;
      this._sourceCode = sourceCode;
      this._visitingId = pathway;
      this._maxDomainsPerVendor = maxDomainsPerVendor;
      this._tlds = tlds;
      this._supportedLanguages = supportedLanguages;
    }

    public override string GetCacheMD5()
    {
      throw new System.NotImplementedException();
    }

    public override string ToXML()
    {
      /*
        sample request:
        <dppdomainsearch 
          vendorid="2,1,4" 
          domainname="fun.com"
          requestingserver="c1wsdv-rphil" 
          customerip="172.18.172.26" 
          privatelabel="1" 
          sourcecode="DPP Avail Check" 
          visitingid="" 
          searchdatabase="premium,auctions"
          maxdomainspervendor="5" 
          tlds="com,net,org" 
          supportedLanguages=="en,es,it,fr,de,pt"
      */
      using (StringWriter sw = new StringWriter())
      {
        using (XmlTextWriter writer = new XmlTextWriter(sw))
        {
          writer.WriteStartElement("dppdomainsearch");
          string vendors = GetVendorList();

          if (!string.IsNullOrEmpty(vendors))
          {
            writer.WriteAttributeString("vendorid", vendors);
          }

          writer.WriteAttributeString("domainname", this._domainName);
          writer.WriteAttributeString("requestingserver", this._requestingServer);
          writer.WriteAttributeString("customerip", this._customerIp);
          writer.WriteAttributeString("privatelabel", this._privateLabelID.ToString());
          writer.WriteAttributeString("sourcecode", this._sourceCode);
          writer.WriteAttributeString("visitingid", this._visitingId);
          writer.WriteAttributeString("searchdatabase", this._searchDatabase);
          writer.WriteAttributeString("maxdomainspervendor", this._maxDomainsPerVendor.ToString());
          writer.WriteAttributeString("searchtimeoutmilliseconds", this._requestTimeout.Subtract(new TimeSpan(0, 0, 1)).TotalMilliseconds.ToString());
          writer.WriteAttributeString("tlds", this._tlds);
          writer.WriteAttributeString("supportedLanguages", this._supportedLanguages);
          writer.WriteEndElement();
        }

        return sw.ToString();
      }
    }

    private string GetVendorList()
    {
      string vendorList = string.Empty;

      if (this._vendorList != null)
      {
        foreach (RegVendorDomainSearchVendor vendorId in this._vendorList)
        {
          vendorList += ((int)vendorId).ToString() + ",";
        }
      }

      return vendorList.TrimEnd(new char[] { ',' });
    }
  }
}
