using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Atlantis.Framework.HCC.Interface
{
  [DataContract]
  public class HCCAccounts : IHCCResponseMessage
  {
    string _responseMessage;
    string _responseStatus;
    int _responseStatusCode;
    string _xml = string.Empty;

    [DataMember]
    ReadOnlyCollection<HCCAccount> _hccAccounts;

    public HCCAccounts(string responseMessage, string responseStatus, int responseStatusCode)
    {
      _responseMessage = responseMessage;
      _responseStatus = responseStatus;
      _responseStatusCode = responseStatusCode;
    }

    [DataMember(Name = "accts")]
    public ReadOnlyCollection<HCCAccount> AccountList
    {
      get
      {
        return _hccAccounts;
      }
      private set { }
    }

    public void SetAccountList(ReadOnlyCollection<HCCAccount> accounts)
    {
      _hccAccounts = accounts;
    }

    public string ToXML()
    {
      if (string.IsNullOrEmpty(_xml))
      {
        XElement accounts = new XElement("accounts");

        foreach (HCCAccount account in AccountList)
        {
          accounts.Add(
            new XElement("account",
              new XAttribute("accountexec", account.HasAccountExecutive),
              new XAttribute("accountuid", account.AccountUid),
              new XAttribute("bandwidthinmb", account.BandwidthInMb),
              new XAttribute("diskspaceinmb", account.DiskspaceInMb),
              new XAttribute("domain", account.Domain),
              new XAttribute("operatingsystem", account.OperatingSystem),
              new XAttribute("Plan", account.Plan),
              new XAttribute("status", account.Status)
            )
          );
        }

        XElement response = new XElement("response",
           new XElement("message", _responseMessage),
           new XElement("status", _responseStatus),
           new XElement("statuscode", _responseStatusCode),
           accounts
        );

        _xml = response.ToString();
      }

      return _xml;
    }

    #region HCCMessage Interface
    public string GetResponseMessage()
    {
      return _responseMessage;
    }

    public string GetResponseStatus()
    {
      return _responseStatus;
    }

    public int GetResponseStatusCode()
    {
      return _responseStatusCode;
    }
    #endregion

    private ExtensionDataObject extensionDataObjectValue;
    public ExtensionDataObject ExtensionData
    {
      get
      {
        return extensionDataObjectValue;
      }
      set
      {
        extensionDataObjectValue = value;
      }
    }

  }
}
