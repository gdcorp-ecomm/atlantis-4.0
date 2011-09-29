using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetDomainInfoByID.Interface
{
  public class DCCGetDomainInfoByIDResponseData: IResponseData
  {
    private readonly AtlantisException _exception;

    public string ResponseXml { get; private set; }

    public int DomainId { get; private set; }

    public string DomainName { get; private set; }

    public int LockedStatus { get; private set; }

    public int RenewalStatus { get; private set; }

    public int ProxyStatus { get; private set; }

    public int Status { get; private set; }

    public int IsExpirationProtected { get; private set; }

    public int IsTransferProtected { get; private set; }

    public string ValidationMessage { get; private set; }

    private bool _isSuccess;
    public bool IsSuccess
    {
      get { return (_exception == null && _isSuccess); }
    }

    private string ShopperId { get; set; }

    private string DomainShopperId { get; set; }

    public DCCGetDomainInfoByIDResponseData(string responseXml, RequestData oRequestData)
    {
      ShopperId = oRequestData.ShopperID;
      ResponseXml = responseXml;
      LockedStatus = 0;
      RenewalStatus = 0;


      PopulateFromXML(responseXml);
    }

    public DCCGetDomainInfoByIDResponseData(string responseXml, AtlantisException exAtlantis)
    {
      ResponseXml = responseXml;
      _exception = exAtlantis;
    }

    public DCCGetDomainInfoByIDResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      ResponseXml = responseXml;
      _exception = new AtlantisException(oRequestData,
                                   "DCCGetDomainInfoByIDResponseData", 
                                   ex.Message, 
                                   oRequestData.ToXML());
    }

    /*
    <results>
      <method>GetDomainInfoByID</method>
      <success>1</success>
      <domains>
      <domain id="1666019" domainname="0kajsdhfjkahsdf.com" shopperid="839627" islocked="0" autorenewflag="1" processing="success">
      </domain></domains>
    </results>
    */

    void PopulateFromXML(string responseXml)
    {
      XmlDocument xdDoc = new XmlDocument();
      xdDoc.LoadXml(responseXml);

      XmlElement xnSuccess = xdDoc.SelectSingleNode("/results/success") as XmlElement;
      if (xnSuccess != null)
      {
        if( xnSuccess.InnerText == "1" )
        {
          _isSuccess = true;
        }
      }

      XmlElement oDomain = xdDoc.SelectSingleNode("/results/domains/domain") as XmlElement;
      if(oDomain != null)
      {
        DomainName = GetAttributeStringValue(oDomain.Attributes["domainname"]);
        DomainId = GetAttributeIntValue(oDomain.Attributes["id"]);
        DomainShopperId = GetAttributeStringValue(oDomain.Attributes["shopperid"]);
        LockedStatus = GetAttributeIntValue(oDomain.Attributes["islocked"]);
        RenewalStatus = GetAttributeIntValue(oDomain.Attributes["autorenewflag"]);
        ProxyStatus = GetAttributeIntValue(oDomain.Attributes["isproxied"]);
        Status = GetAttributeIntValue(oDomain.Attributes["status"]);
        IsExpirationProtected = GetAttributeIntValue(oDomain.Attributes["isexpirationprotected"]);
        IsTransferProtected = GetAttributeIntValue(oDomain.Attributes["istransferprotected"]);
      }

      ValidateDomain();
    }

    private void ValidateDomain()
    {
      if(ShopperId != DomainShopperId)
      {
        DomainName = string.Empty;
        DomainShopperId = string.Empty;
        LockedStatus = 0;
        RenewalStatus = 0;
        ProxyStatus = 0;
        Status = 0;
        IsExpirationProtected = 0;
        IsTransferProtected = 0;

        _isSuccess = false;
        ValidationMessage = "Invalid request.";
      }
    }

    private static string GetAttributeStringValue(XmlAttribute attribute)
    {
      string value = string.Empty;
      if(attribute != null && !string.IsNullOrEmpty(attribute.Value))
      {
        value = attribute.Value;
      }
      return value;
    }

    private static int GetAttributeIntValue(XmlAttribute attribute)
    {
      int value = 0;
      if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
      {
        int.TryParse(attribute.Value, out value);
      }
      return value;
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return ResponseXml;
    }

    #endregion
  }
}
