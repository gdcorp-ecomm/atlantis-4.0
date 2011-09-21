using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Atlantis.Framework.HCC.Interface
{
  [DataContract(Name = "hccresp")]
  public class HCCGet404ErrorBehaviorResponse : IHCCResponseMessage
  {
    [DataMember(Name = "xml")]
    string _xml = string.Empty;

    [DataMember(Name = "msg")]
    string _responseMessage;
    
    [DataMember(Name = "status")]
    string _responseStatus;
    
    [DataMember(Name = "code")]
    int _responseStatusCode;

    public HCCGet404ErrorBehaviorResponse(string responseMessage, string responseStatus, int responseStatusCode)
    {
      _responseMessage = responseMessage;
      _responseStatus = responseStatus;
      _responseStatusCode = responseStatusCode;
    }

    [DataMember(Name="pagetype")]
    public string PageType { get; set; }

    [DataMember(Name="path")]
    public string Path { get; set; }

    [DataMember(Name="filename")]
    public string FileName { get; set; }

    [DataMember(Name="errornumber")]
    public int ErrorNumber { get; set; }

    public string ToXML()
    {
      if (string.IsNullOrEmpty(_xml))
      {
        XElement response = new XElement("response",
           new XElement("message", _responseMessage),
           new XElement("status", _responseStatus),
           new XElement("statuscode", _responseStatusCode),
           new XElement("pagetype", PageType),
           new XElement("path", Path),
           new XElement("filename", FileName),
           new XElement("errornumber", ErrorNumber)
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
