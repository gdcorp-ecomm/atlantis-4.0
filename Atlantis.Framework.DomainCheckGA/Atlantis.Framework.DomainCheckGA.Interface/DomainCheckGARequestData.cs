using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainCheckGA.Interface
{
  public class DomainCheckGARequestData : RequestData
  {
    const string _defaultCheckDataType = "GA";
    bool _skipZoneCheck = false;
    bool _skipDatabaseCheck = false;
    bool _skipRegistryCheck = false;

    int _privateLabelID = -1;
    string _clientIPAddress = string.Empty;
    string _registrarID = string.Empty;
    string _sourceCode = string.Empty;
    string _hostIPAddress = string.Empty;

    TimeSpan _waitTime = TimeSpan.FromMilliseconds(2500);

    List<DomainToCheckGA> _domainsToCheck = new List<DomainToCheckGA>();
    HashSet<string> _addedDomains = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

    public DomainCheckGARequestData(
      string shopperID, string sourceUrl, string orderId, string pathway, int pageCount,
      int privateLabelId, string clientIPAddress, string sourceCode)
      : base(shopperID, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(2500);
      _privateLabelID = privateLabelId;
      ClientIPAddress = clientIPAddress;
      _sourceCode = sourceCode;
      _hostIPAddress = GetLocalAddress();
    }

    public DomainCheckGARequestData(
      string shopperID, string sourceUrl, string orderId, string pathway, int pageCount,
      DomainToCheckGA domain, int privateLabelId, string clientIPAddress, string sourceCode)
      : this(shopperID, sourceUrl, orderId, pathway, pageCount, privateLabelId, clientIPAddress, sourceCode)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(2500);
      AddDomain(domain);
    }

    public DomainCheckGARequestData(
      string shopperID, string sourceUrl, string orderId, string pathway, int pageCount,
      IEnumerable<DomainToCheckGA> domains, int privateLabelId, string clientIPAddress, string sourceCode)
      : this(shopperID, sourceUrl, orderId, pathway, pageCount, privateLabelId, clientIPAddress, sourceCode)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(2500);
      AddDomains(domains);
    }

    public bool SkipZoneCheck
    {
      get { return _skipZoneCheck; }
      set { _skipZoneCheck = value; }
    }

    public bool SkipDatabaseCheck
    {
      get { return _skipDatabaseCheck; }
      set { _skipDatabaseCheck = value; }
    }

    public bool SkipRegistryCheck
    {
      get { return _skipRegistryCheck; }
      set { _skipRegistryCheck = value; }
    }

    public string ClientIPAddress
    {
      get { return _clientIPAddress; }
      private set
      {
        _clientIPAddress = string.Empty;
        IPAddress address = null;
        if (IPAddress.TryParse(value, out address))
          _clientIPAddress = address.ToString();
      }
    }

    public string HostIPAddress
    {
      get { return _hostIPAddress; }
    }

    public string RegistrarID
    {
      get { return _registrarID; }
      set { _registrarID = value; }
    }

    public int PrivateLabelID
    {
      get { return _privateLabelID; }
    }

    public TimeSpan WaitTime
    {
      get { return _waitTime; }
      set { _waitTime = value; }
    }

    [Obsolete("Please use RequestTimeout instead.")]
    public TimeSpan ServiceTimeout
    {
      get { return RequestTimeout; }
      set { RequestTimeout = value; }
    }
    
    public string SourceCode
    {
      get { return _sourceCode; }
      set { _sourceCode = value; }
    }

    string _checkDataType;
    /// <summary>
    /// Search type used for mapping. if no type is set, type default is "GA".
    /// </summary>
    public string CheckDataType
    {
      get 
      {
        if (string.IsNullOrEmpty(_checkDataType))
        {
          _checkDataType = _defaultCheckDataType;
        }

        return _checkDataType; 
      }
      set { _checkDataType = value; }
    }

    public void AddDomain(DomainToCheckGA domainToCheck)
    {
      if (!_addedDomains.Contains(domainToCheck.DomainName))
      {
        _domainsToCheck.Add(domainToCheck);
        _addedDomains.Add(domainToCheck.DomainName);
      }
    }

    public void AddDomains(IEnumerable<DomainToCheckGA> domainsToCheck)
    {
      foreach (DomainToCheckGA domainToCheck in domainsToCheck)
      {
        AddDomain(domainToCheck);
      }
    }

    string GetLocalAddress()
    {
      string sLocalAddress = string.Empty;

      IPAddress[] addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

      if (addresses.Length > 0)
        sLocalAddress = addresses[0].ToString();

      return sLocalAddress;
    }

    #region RequestData Members

    private string GetRegistrarID()
    {
      string result = "2";
      if (_privateLabelID == 1)
        result = "1";
      else if (_privateLabelID == 2)
        result = "3";
      return result;
    }

    public override string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwResult = new XmlTextWriter(new StringWriter(sbResult));

      xtwResult.WriteStartElement("checkdata");

      xtwResult.WriteAttributeString("waittime", WaitTime.TotalMilliseconds.ToString());

      xtwResult.WriteAttributeString("type", CheckDataType);

      StringBuilder sbSkip = new StringBuilder(SkipZoneCheck ? "checkzonefile" : string.Empty);
      sbSkip.Append((sbSkip.Length > 0 ? "," : string.Empty) + (SkipDatabaseCheck ? "checkdb" : string.Empty));
      sbSkip.Append((sbSkip.Length > 0 ? "," : string.Empty) + (SkipRegistryCheck ? "checkregistry" : string.Empty));

      if (sbSkip.Length > 0)
        xtwResult.WriteAttributeString("skip", sbSkip.ToString());

      if (ClientIPAddress.Length > 0)
        xtwResult.WriteAttributeString("customerIP", ClientIPAddress);

      if (PrivateLabelID > -1)
        xtwResult.WriteAttributeString("privatelabelid", PrivateLabelID.ToString());

      if (HostIPAddress.Length > 0)
        xtwResult.WriteAttributeString("ip", HostIPAddress);

      xtwResult.WriteAttributeString("source", Environment.MachineName);

      if (RegistrarID.Length > 0)
        xtwResult.WriteAttributeString("registrarid", RegistrarID);
      else
        xtwResult.WriteAttributeString("registrarid", GetRegistrarID());

      if (!string.IsNullOrEmpty(_sourceCode))
        xtwResult.WriteAttributeString("sourceCode", _sourceCode);

      if (Pathway.Length > 0)
        xtwResult.WriteAttributeString("visitingID", Pathway);

      foreach (DomainToCheckGA domainToCheck in _domainsToCheck)
      {
        xtwResult.WriteStartElement("domain");
        xtwResult.WriteAttributeString("name", domainToCheck.DomainName);

        if (!string.IsNullOrEmpty(domainToCheck.LanguageTag))
        {
          xtwResult.WriteAttributeString("idnScript", domainToCheck.LanguageTag);
        }

        if (domainToCheck.WasTyped)
        {
          xtwResult.WriteAttributeString("wasTyped", "1");
        }

        xtwResult.WriteEndElement(); // domain
      }

      xtwResult.WriteEndElement(); // checkdata

      return sbResult.ToString();
    }

    public override string GetCacheMD5()
    {
      throw new Exception("DomainCheckGA is not a chacheable request.");
    }

    #endregion
  }
}
