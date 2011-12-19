using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.MerchantAccountActivate.Interface
{
  public class MerchantAccountActivateResponseData : IResponseData, ISessionSerializableResponse
  {
    #region Properties
    private readonly AtlantisException _exception;
    private bool _success;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public string AuthenticationGuid { get; private set; }
    #endregion

    public MerchantAccountActivateResponseData(string authenticationGuid)
    {
      AuthenticationGuid = authenticationGuid;
      _success = true;
    }

    public MerchantAccountActivateResponseData() 
    { }

    public MerchantAccountActivateResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public MerchantAccountActivateResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "MerchantAccountActivateResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return new XElement("authentication_guid", AuthenticationGuid).ToString();
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
      XDocument xDoc = XDocument.Parse(sessionData);

      AuthenticationGuid = xDoc.Element("authentication_guid").Value.ToString();

      if (!string.IsNullOrWhiteSpace(AuthenticationGuid))
      {
        _success = true;
      }
    }
    #endregion
  }
}
