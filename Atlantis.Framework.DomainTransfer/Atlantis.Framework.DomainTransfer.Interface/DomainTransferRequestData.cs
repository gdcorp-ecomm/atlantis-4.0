using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainTransfer.Interface
{
  public class DomainTransferRequestData : RequestData
  {
    bool _skipZoneCheck = false;
    bool _skipDatabaseCheck = false;
    bool _skipRegistryCheck = false;

    int _privateLabelID = -1;
    string _clientIPAddress = string.Empty;
    string _registrarID = string.Empty;
    string _sourceCode = string.Empty;
    string _hostIPAddress = string.Empty;

    TimeSpan _waitTime = TimeSpan.FromMilliseconds(5000);

    List<TransferToCheck> _transfersToCheck = new List<TransferToCheck>();
    HashSet<string> _addedDomains = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

    public DomainTransferRequestData(
      string shopperID, string sourceUrl, string orderId, string pathway, int pageCount,
      int privateLabelId, string clientIPAddress, string sourceCode)
      : base(shopperID, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(5000);
      _privateLabelID = privateLabelId;
      _clientIPAddress = clientIPAddress;
      _sourceCode = sourceCode;
      _hostIPAddress = GetLocalAddress();

    }

    public DomainTransferRequestData(
      string shopperID, string sourceUrl, string orderId, string pathway, int pageCount,
      TransferToCheck domain, int privateLabelId, string clientIPAddress, string sourceCode)
      : this(shopperID, sourceUrl, orderId, pathway, pageCount, privateLabelId, clientIPAddress, sourceCode)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(5000);
      AddDomain(domain);
    }

    public DomainTransferRequestData(
      string shopperID, string sourceUrl, string orderId, string pathway, int pageCount,
      IEnumerable<TransferToCheck> domains, int privateLabelId, string clientIPAddress, string sourceCode)
      : this(shopperID, sourceUrl, orderId, pathway, pageCount, privateLabelId, clientIPAddress, sourceCode)
    {
      RequestTimeout = TimeSpan.FromMilliseconds(5000);
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

    public void AddDomain(TransferToCheck transferToCheck)
    {
      if (!_addedDomains.Contains(transferToCheck.DomainName))
      {
        _transfersToCheck.Add(transferToCheck);
        _addedDomains.Add(transferToCheck.DomainName);
      }
    }

    public void AddDomains(IEnumerable<TransferToCheck> transfersToCheck)
    {
      foreach (TransferToCheck transferToCheck in transfersToCheck)
      {
        AddDomain(transferToCheck);
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

      xtwResult.WriteAttributeString("type", "XFR");

      StringBuilder sbSkip = new StringBuilder(SkipZoneCheck ? "checkzonefile" : string.Empty);
      sbSkip.Append((sbSkip.Length > 0 ? "," : string.Empty) + (SkipDatabaseCheck ? "checkdb" : string.Empty));
      sbSkip.Append((sbSkip.Length > 0 ? "," : string.Empty) + (SkipRegistryCheck ? "checkregistry" : string.Empty));

      if(sbSkip.Length > 0)
        xtwResult.WriteAttributeString("skip", sbSkip.ToString());

      if (ClientIPAddress.Length > 0)
        xtwResult.WriteAttributeString("customerIP", ClientIPAddress);

      if (PrivateLabelID > -1)
        xtwResult.WriteAttributeString("privatelabelid", PrivateLabelID.ToString());

      if (HostIPAddress.Length > 0)
        xtwResult.WriteAttributeString("ip", HostIPAddress);

      xtwResult.WriteAttributeString("source", Environment.MachineName);

      if (RegistrarID.Length > 0)
        xtwResult.WriteAttributeString("registrarID", RegistrarID);
      else
        xtwResult.WriteAttributeString("registrarID", GetRegistrarID());

      if (!string.IsNullOrEmpty(_sourceCode))
        xtwResult.WriteAttributeString("sourceCode", _sourceCode);

      if (Pathway.Length > 0)
        xtwResult.WriteAttributeString("visitingID", Pathway);

      foreach (TransferToCheck transferToCheck in _transfersToCheck)
      {
        xtwResult.WriteStartElement("domain");
        xtwResult.WriteAttributeString("name", transferToCheck.DomainName);

        if (!string.IsNullOrEmpty(transferToCheck.LanguageTag))
        {
          xtwResult.WriteAttributeString("idnScript", transferToCheck.LanguageTag);
        }

        if (transferToCheck.WasTyped)
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
      throw new Exception("DomainTransfer is not a chacheable request.");
    }

    #endregion

  }
}
