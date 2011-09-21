using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Atlantis.Framework.HCC.Interface
{
  [DataContract(Name = "hccresp")]
  public class HCCAccountDetailsResponse : IHCCResponseMessage
  {
    [DataMember(Name = "xml")]
    string _xml = string.Empty;

    [DataMember(Name = "msg")]
    string _responseMessage;
    
    [DataMember(Name = "status")]
    string _responseStatus;
    
    [DataMember(Name = "code")]
    int _responseStatusCode;

    public HCCAccountDetailsResponse(string responseMessage, string responseStatus, int responseStatusCode)
    {
      _responseMessage = responseMessage;
      _responseStatus = responseStatus;
      _responseStatusCode = responseStatusCode;
    }

    [DataMember(Name = "os")]
    public string OperatingSystem { get; set; }
    
    [DataMember(Name = "bwallot")]
    public string BandwidthAllotment {get; set; }

    [DataMember(Name = "dsallot")]
    public string DiskspaceAllotment {get; set; }

    [DataMember(Name = "mysql")]
    public int MySqlDatabasesAvailable {get; set; }

    [DataMember(Name = "mssql")]
    public int MSSqlDatabasesAvailable {get; set; }

    [DataMember(Name = "dom")]
    public string Domain {get; set; }

    [DataMember(Name = "isfreehost")]
    public bool IsFreeHosting {get; set; }

    [DataMember(Name = "prodnam")]
    public string ProductName {get; set; }

    public string ToXML()
    {
      if (string.IsNullOrEmpty(_xml))
      {
        XElement response = new XElement("response",
           new XElement("message", _responseMessage),
           new XElement("status", _responseStatus),
           new XElement("statuscode", _responseStatusCode),
           new XElement("operatingsystem", OperatingSystem),
           new XElement("bandwidthallotment", BandwidthAllotment),
           new XElement("diskspaceallotment", DiskspaceAllotment),
           new XElement("mysqldatabasesavailable", MySqlDatabasesAvailable),
           new XElement("mssqldatabasesavailable", MSSqlDatabasesAvailable),
           new XElement("isfreehosting", IsFreeHosting),
           new XElement("productname", ProductName)
          );                   

        _xml = response.ToString();
      }

      return _xml;
    }

    #region IHCCResponseMessage Interface
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
  }
}
