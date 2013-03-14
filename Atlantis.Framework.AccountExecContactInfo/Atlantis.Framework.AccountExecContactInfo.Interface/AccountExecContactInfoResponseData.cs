using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.AccountExecContactInfo.Interface
{
  public class AccountExecContactInfoResponseData : IResponseData, ISessionSerializableResponse
  {
    private readonly AtlantisException _exception;
    public VipInfo VipRepInfo { get; private set; }

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public AccountExecContactInfoResponseData()
    {
    }

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
      string xml = string.Empty;
      ;

      if (VipRepInfo != null)
      {
        var vipInfoXml = new XElement("vipInfo"
                                      , new XAttribute("name", VipRepInfo.RepName)
                                      , new XAttribute("email", VipRepInfo.RepEmail)
                                      , new XAttribute("portfoliotypeid", VipRepInfo.RepPortfolioTypeId)
                                      , new XAttribute("portfoliotype", VipRepInfo.RepPortfolioType)
                                      , new XAttribute("phone", VipRepInfo.RepExternalContactPhone)
                                      , new XAttribute("extension", VipRepInfo.RepPhoneExtension));

        xml = vipInfoXml.ToString();
      }

      return xml;
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
        var xDoc = XDocument.Parse(sessionData);
        var vipXml = xDoc.Element("vipInfo");
        if (vipXml != null)
        {
          VipRepInfo = new VipInfo(vipXml.Attribute("name").Value
                                   , vipXml.Attribute("email").Value
                                   , (PortfolioTypes)Enum.Parse(typeof (PortfolioTypes), vipXml.Attribute("portfoliotypeid").Value)
                                   , vipXml.Attribute("portfoliotype").Value
                                   , vipXml.Attribute("phone").Value
                                   , vipXml.Attribute("extension").Value);
        }
      }
    }

    #endregion

  }
}