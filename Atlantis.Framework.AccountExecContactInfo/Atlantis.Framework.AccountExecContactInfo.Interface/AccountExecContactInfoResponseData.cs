using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.AccountExecContactInfo.Interface
{
  public class AccountExecContactInfoResponseData : IResponseData, ISessionSerializableResponse
  {
    private AtlantisException _exception = null;
    public VipInfo VipRepInfo { get; private set; }

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public AccountExecContactInfoResponseData()
    { }

    public AccountExecContactInfoResponseData(VipInfo vipInfo)
    {
      VipRepInfo = vipInfo;
    }

     public AccountExecContactInfoResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public AccountExecContactInfoResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "AccountExecContactInfoResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      XElement vipInfoXml = new XElement("vipInfo" 
        , new XAttribute("name", VipRepInfo.RepName)
        , new XAttribute("email", VipRepInfo.RepEmail)
        , new XAttribute("portfoliotype", VipRepInfo.RepPortfolioType)
        , new XAttribute("phone", VipRepInfo.RepExternalContactPhone)
        , new XAttribute("extension", VipRepInfo.RepPhoneExtension));

      return vipInfoXml.ToString();
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
      if (!string.IsNullOrEmpty(sessionData))
      {
        XDocument xDoc = XDocument.Parse(sessionData);
        XElement vipXml = xDoc.Element("vipInfo");
        VipRepInfo = new VipInfo(vipXml.Attribute("name").Value
          , vipXml.Attribute("email").Value
          , vipXml.Attribute("portfoliotype").Value
          , vipXml.Attribute("phone").Value
          , vipXml.Attribute("extension").Value);
      }
    }
    #endregion
 
  }
}
