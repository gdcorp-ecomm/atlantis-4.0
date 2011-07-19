using System;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.MessagingShowUpdateEmail.Interface
{
  public class MessagingShowUpdateEmailResponseData : IResponseData, ISessionSerializableResponse
  {
    #region Properties
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    public bool ShowUpdateEmailInfo { get; set; }
    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    #endregion

    public MessagingShowUpdateEmailResponseData()
    { }

    public MessagingShowUpdateEmailResponseData(bool showUpdateEmailInfo)
    {
      ShowUpdateEmailInfo = showUpdateEmailInfo;
      _resultXML = new XElement("updateemail",
        new XAttribute("show", showUpdateEmailInfo.ToString())
      ).ToString();
    }

     public MessagingShowUpdateEmailResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

     public MessagingShowUpdateEmailResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "MessagingShowUpdateEmailResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      _resultXML = sessionData;
      XmlDocument result = new XmlDocument();
      result.LoadXml(sessionData);

      XmlNode root = result.SelectSingleNode("updateemail");

      bool showUpdateEmail;
      if (bool.TryParse(root.Attributes["show"].Value, out showUpdateEmail))
      {
        ShowUpdateEmailInfo = showUpdateEmail;
      }
    }
    #endregion

  }
}
