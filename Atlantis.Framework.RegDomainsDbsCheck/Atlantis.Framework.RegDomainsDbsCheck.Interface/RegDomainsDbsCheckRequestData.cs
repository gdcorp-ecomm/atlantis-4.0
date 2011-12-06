using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RegDomainsDbsCheck.Interface
{
  public class RegDomainsDbsCheckRequestData : RequestData
  {
    #region Properties

    private const int kDefaultTimeout = 2500;
    private HashSet<string> _domainNames = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

    #endregion Properties

    #region Constructors

    public RegDomainsDbsCheckRequestData(string sShopperID,
                                         string sSourceURL,
                                         string sOrderID,
                                         string sPathway,
                                         int iPageCount)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(kDefaultTimeout);
    }

    public RegDomainsDbsCheckRequestData(string sShopperID,
                                         string sSourceURL,
                                         string sOrderID,
                                         string sPathway,
                                         int iPageCount,
                                         string domainName)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(kDefaultTimeout);
      AddDomainName(domainName);
    }

    public RegDomainsDbsCheckRequestData(string sShopperID,
                                         string sSourceURL,
                                         string sOrderID,
                                         string sPathway,
                                         int iPageCount,
                                         IEnumerable<string> domainNames)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(kDefaultTimeout);
      AddDomainNames(domainNames);
    }

    #endregion Constructors

    #region Public Methods

    public void AddDomainName(string domainName)
    {
      if (!this._domainNames.Contains(domainName))
      {
        this._domainNames.Add(domainName);
      }
    }

    public void AddDomainNames(IEnumerable<string> domainNames)
    {
      foreach (string name in domainNames)
      {
        AddDomainName(name);
      }
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));
      xtwRequest.WriteStartElement("domains");

      foreach (string name in this._domainNames)
      {
        xtwRequest.WriteStartElement("domain");
        xtwRequest.WriteString(name);
        xtwRequest.WriteEndElement();
      }

      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }

    #endregion Public Methods

    #region Private Methods
    #endregion Private Methods
  }
}
