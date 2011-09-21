using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Atlantis.Framework.HCC.Interface
{
  [DataContract]
  public class HCCAcctPasswordResponse : IHCCResponseMessage
  {
    string _responseMessage;
    string _responseStatus;
    int _responseStatusCode;
    string _xml = string.Empty;

    public HCCAcctPasswordResponse(string responseMessage, string responseStatus, int responseStatusCode)
    {
      _responseMessage = responseMessage;
      _responseStatus = responseStatus;
      _responseStatusCode = responseStatusCode;
    }

    [DataMember]
    public bool IsAccountsInSync { get; set; }

    [DataMember]
    public string[] Errors { get; set; }

    public string ToXML()
    {
      if (string.IsNullOrEmpty(_xml))
      {
        XElement errors = new XElement("errors");

        foreach (string error in Errors)
        {
          errors.Add(
            new XElement("error", error)
          );
        }

        XElement response = new XElement("response",
           new XElement("message", _responseMessage),
           new XElement("status", _responseStatus),
           new XElement("statuscode", _responseStatusCode),
           new XElement("isaccountsinsync", IsAccountsInSync),
           errors
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
  }
}
