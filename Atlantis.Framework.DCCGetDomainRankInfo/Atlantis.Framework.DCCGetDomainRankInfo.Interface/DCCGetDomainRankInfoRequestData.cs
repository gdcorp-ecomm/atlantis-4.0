using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetDomainRankInfo.Interface
{
  public class DCCGetDomainRankInfoRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = new TimeSpan(0, 0, 5);

    public Dictionary<int, string> DomainData { get; private set; }

    public string AppName { get; private set; }

    private bool ProvideLinkToDiagnostics { get; set; }

    public DCCGetDomainRankInfoRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , string appName
      , bool provideLinkToDiagnostics
      , Dictionary<int, string> domainData)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)    
    {
      AppName = appName;
      DomainData = domainData;
      ProvideLinkToDiagnostics = provideLinkToDiagnostics;
      RequestTimeout = _requestTimeout;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}", ShopperID));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }

    public override string ToXML()
    {
      int includeMainUrl = Convert.ToInt32(ProvideLinkToDiagnostics);
      XElement root = new XElement("request");
      root.Add(new XElement("callingApplicationName", AppName));
      root.Add(new XElement("shopperId", ShopperID));

      XElement domains = new XElement("domains");

      foreach (int domainId in DomainData.Keys)
      {
        string domainName;
        if (!DomainData.TryGetValue(domainId, out domainName))
        {
          domainName = string.Empty;
        }

        XElement domain = new XElement("domain",
          new XAttribute("id", domainId),
          new XAttribute("name", domainName),
          new XAttribute("includeMainUrl", includeMainUrl));

        domains.Add(domain);
      }

      root.Add(domains);

      return root.ToString();      
    }
  }
}
