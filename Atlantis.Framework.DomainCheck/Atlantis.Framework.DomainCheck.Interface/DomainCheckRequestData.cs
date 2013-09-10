using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainCheck.Interface
{
  public class DomainCheckRequestData : RequestData
  {
    readonly int _privateLabelID = -1;
    string _clientIpAddress = string.Empty;
    string _registrarId = string.Empty;
    string _sourceCode = string.Empty;
    string _typedDomainName = string.Empty;
    string _tldChoice = string.Empty;
    string _splitValue = string.Empty;
    bool _wasTldSelected;
    bool _specialCharsRemoved;

    readonly string _hostIpAddress = string.Empty;

    TimeSpan _waitTime = TimeSpan.FromMilliseconds(2500);

    readonly List<DomainToCheck> _domainsToCheck = new List<DomainToCheck>();
    readonly HashSet<string> _addedDomains = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

    public DomainCheckRequestData(
      string shopperID, string sourceUrl, string orderId, string pathway, int pageCount,
      int privateLabelId, string clientIPAddress, string sourceCode)
      : base(shopperID, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(2500);
      _privateLabelID = privateLabelId;
      ClientIPAddress = clientIPAddress;
      _sourceCode = sourceCode;
      _hostIpAddress = GetLocalAddress();
    }

    public DomainCheckRequestData(
      string shopperID, string sourceUrl, string orderId, string pathway, int pageCount,
      DomainToCheck domain, int privateLabelId, string clientIPAddress, string sourceCode)
      : this(shopperID, sourceUrl, orderId, pathway, pageCount, privateLabelId, clientIPAddress, sourceCode)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(2500);
      AddDomain(domain);
      _specialCharsRemoved = domain.SpecialCharsRemoved;
      _typedDomainName = domain.TypedDomainName;
      _tldChoice = domain.TldChoice;
      _wasTldSelected = domain.WasTldSelected;
      _splitValue = domain.SplitValue;
    }

    public DomainCheckRequestData(
      string shopperID, string sourceUrl, string orderId, string pathway, int pageCount,
      IEnumerable<DomainToCheck> domains, int privateLabelId, string clientIPAddress, string sourceCode)
      : this(shopperID, sourceUrl, orderId, pathway, pageCount, privateLabelId, clientIPAddress, sourceCode)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(2500);
      AddDomains(domains);
    }

    public bool SkipZoneCheck { get; set; }

    public bool SkipDatabaseCheck { get; set; }

    public bool SkipRegistryCheck { get; set; }

    public string ClientIPAddress
    {
      get { return _clientIpAddress; }
      private set
      {
        _clientIpAddress = string.Empty;
        IPAddress address;

        if (IPAddress.TryParse(value, out address))
        {
          _clientIpAddress = address.ToString();
        }
      }
    }

    public string HostIPAddress
    {
      get { return _hostIpAddress; }
    }

    public string RegistrarID
    {
      get { return _registrarId; }
      set { _registrarId = value; }
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

    public string TypedDomainName
    {
      get { return _typedDomainName; }
      set { _typedDomainName = value; }
    }

    public string TldChoice
    {
      get { return _tldChoice; }
      set { _tldChoice = value; }
    }

    public bool WasTLDSelected
    {
      get { return _wasTldSelected; }
      set { _wasTldSelected = value; }
    }

    public bool SpecialCharsRemoved
    {
      get { return _specialCharsRemoved; }
      set { _specialCharsRemoved = value; }
    }

    public string SplitValue
    {
      get { return _splitValue; }
      set { _splitValue = value; }
    }

    public void AddDomain(DomainToCheck domainToCheck)
    {
      if (_addedDomains.Contains(domainToCheck.DomainName)) return;

      _domainsToCheck.Add(domainToCheck);
      _addedDomains.Add(domainToCheck.DomainName);
    }

    public void AddDomains(IEnumerable<DomainToCheck> domainsToCheck)
    {
      foreach (var domainToCheck in domainsToCheck)
      {
        if (!string.IsNullOrEmpty(domainToCheck.TypedDomainName))
        {
          _specialCharsRemoved = domainToCheck.SpecialCharsRemoved;
          _typedDomainName = domainToCheck.TypedDomainName;
          _tldChoice = domainToCheck.TldChoice;
          _wasTldSelected = domainToCheck.WasTldSelected;
          _splitValue = domainToCheck.SplitValue;
        }

        AddDomain(domainToCheck);
      }
    }

    string GetLocalAddress()
    {
      var sLocalAddress = string.Empty;
      var addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

      if (addresses.Length > 0)
      {
        sLocalAddress = addresses[0].ToString();
      }

      return sLocalAddress;
    }

    #region RequestData Members

    private string GetRegistrarID()
    {
      var result = "2";

      if (_privateLabelID == 1)
      {
        result = "1";
      }
      else if (_privateLabelID == 2)
      {
        result = "3";
      }

      return result;
    }

    public override string ToXML()
    {
      var sbResult = new StringBuilder();
      var xtwResult = new XmlTextWriter(new StringWriter(sbResult));
      xtwResult.WriteStartElement("checkdata");
      xtwResult.WriteAttributeString("waittime", WaitTime.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
      xtwResult.WriteAttributeString("type", "REG");
      var sbSkip = new StringBuilder(SkipZoneCheck ? "checkzonefile" : string.Empty);
      sbSkip.Append((sbSkip.Length > 0 ? "," : string.Empty) + (SkipDatabaseCheck ? "checkdb" : string.Empty));
      sbSkip.Append((sbSkip.Length > 0 ? "," : string.Empty) + (SkipRegistryCheck ? "checkregistry" : string.Empty));

      if (sbSkip.Length > 0)
      {
        xtwResult.WriteAttributeString("skip", sbSkip.ToString());
      }

      if (ClientIPAddress.Length > 0)
      {
        xtwResult.WriteAttributeString("customerIP", ClientIPAddress);
      }

      if (!string.IsNullOrEmpty(ShopperID))
      {
        xtwResult.WriteAttributeString("shopperID", ShopperID);
      }

      if (PrivateLabelID > -1)
      {
        xtwResult.WriteAttributeString("privatelabelid", PrivateLabelID.ToString(CultureInfo.InvariantCulture));
      }

      if (HostIPAddress.Length > 0)
      {
        xtwResult.WriteAttributeString("ip", HostIPAddress);
      }

      xtwResult.WriteAttributeString("source", Environment.MachineName);
      xtwResult.WriteAttributeString("registrarID", RegistrarID.Length > 0 ? RegistrarID : GetRegistrarID());

      if (!string.IsNullOrEmpty(_sourceCode))
      {
        xtwResult.WriteAttributeString("sourceCode", _sourceCode);
      }

      if (Pathway.Length > 0)
      {
        xtwResult.WriteAttributeString("visitingID", Pathway);
      }

      if (!string.IsNullOrEmpty(TypedDomainName))
      {
        xtwResult.WriteAttributeString("typedDomainName", TypedDomainName);
      }

      if (!string.IsNullOrEmpty(TldChoice))
      {
        xtwResult.WriteAttributeString("tldChoice", TldChoice);
      }

      xtwResult.WriteAttributeString("wasTLDSelected", WasTLDSelected ? "1" : "0");

      xtwResult.WriteAttributeString("specialCharsRemoved", SpecialCharsRemoved ? "1" : "0");

      if (!string.IsNullOrEmpty(SplitValue))
      {
        xtwResult.WriteAttributeString("splitValue", SplitValue);
      }

      foreach (var domainToCheck in _domainsToCheck)
      {
        xtwResult.WriteStartElement("domain");
        //xtwResult.WriteAttributeString("name", domainToCheck.DomainName);
        xtwResult.WriteAttributeString("find", domainToCheck.DomainName);

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
      throw new Exception("DomainCheck is not a chacheable request.");
    }

    #endregion
  }
}
