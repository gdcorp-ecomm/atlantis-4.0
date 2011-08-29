using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.NameserverCheck.Interface
{
  public class NameserverCheckRequestData : RequestData
  {
    bool _skipZoneCheck = false;
    bool _skipDatabaseCheck = false;
    bool _skipRegistryCheck = false;

    int _privateLabelID = -1;
    string _clientIPAddress = string.Empty;
    string _registrarID = string.Empty;
    string _sourceCode = string.Empty;
    string _hostIPAddress = string.Empty;

    TimeSpan _waitTime = TimeSpan.FromMilliseconds(2500);

    HashSet<string> _nameServers = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

    public NameserverCheckRequestData(
      string shopperID, string sourceUrl, string orderId, string pathway, int pageCount,
      int privateLabelId, string clientIPAddress, string sourceCode)
      : base(shopperID, sourceUrl, orderId, pathway, pageCount)
    {
      _privateLabelID = privateLabelId;
      _clientIPAddress = clientIPAddress;
      _sourceCode = sourceCode;
      _hostIPAddress = GetLocalAddress();
    }

    public NameserverCheckRequestData(
      string shopperID, string sourceUrl, string orderId, string pathway, int pageCount,
      string nameServer, int privateLabelId, string clientIPAddress, string sourceCode)
      : this(shopperID, sourceUrl, orderId, pathway, pageCount, privateLabelId, clientIPAddress, sourceCode)
    {
      AddNameServer(nameServer);
    }

    public NameserverCheckRequestData(
      string shopperID, string sourceUrl, string orderId, string pathway, int pageCount,
      IEnumerable<string> nameServers, int privateLabelId, string clientIPAddress, string sourceCode)
      : this(shopperID, sourceUrl, orderId, pathway, pageCount, privateLabelId, clientIPAddress, sourceCode)
    {
      AddNameServers(nameServers);
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

    public string SourceCode
    {
      get { return _sourceCode; }
      set { _sourceCode = value; }
    }

    public void AddNameServer(string nameServer)
    {
      _nameServers.Add(nameServer.ToUpperInvariant());
    }

    public void AddNameServers(IEnumerable<string> nameServers)
    {
      foreach (string nameServer in nameServers)
      {
        AddNameServer(nameServer);
      }
    }

    private string GetLocalAddress()
    {
      string sLocalAddress = "";

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

      xtwResult.WriteAttributeString("type", "REG");

      StringBuilder sbSkip = new StringBuilder(SkipZoneCheck ? "checkzonefile" : "");
      sbSkip.Append((sbSkip.Length > 0 ? "," : "") + (SkipDatabaseCheck ? "checkdb" : ""));
      sbSkip.Append((sbSkip.Length > 0 ? "," : "") + (SkipRegistryCheck ? "checkregistry" : ""));

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
        xtwResult.WriteAttributeString("registrarID", RegistrarID);
      else
        xtwResult.WriteAttributeString("registrarID", GetRegistrarID());

      foreach (string nameServer in _nameServers)
      {
        xtwResult.WriteStartElement("host");
        xtwResult.WriteAttributeString("name", nameServer);
        xtwResult.WriteAttributeString("result", "0");
        xtwResult.WriteEndElement();
      }

      xtwResult.WriteEndElement();

      return sbResult.ToString();
    }

    public override string GetCacheMD5()
    {
      throw new Exception("NameserverCheck is not a cacheable request.");
    }

    #endregion
  }
}
