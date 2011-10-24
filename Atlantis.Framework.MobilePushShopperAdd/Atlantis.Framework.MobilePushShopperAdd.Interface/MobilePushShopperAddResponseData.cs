using System;
using System.Reflection;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobilePushShopperAdd.Interface
{
  public class MobilePushShopperAddResponseData : IResponseData
  {
    private AtlantisException AtlantisException { get; set; }

    public string Xml { get; set; }

    public bool IsSuccess { get; private set; }

    public MobilePushShopperAddResponseData(MobilePushShopperAddRequestData requestData, string responseXml)
    {
      Xml = responseXml;
      if (!ParseResponseXml(responseXml))
      {
        IsSuccess = false;
        AtlantisException = new AtlantisException(requestData,
                                                  MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                  string.Format("Unable to parse xml. {0}", responseXml),
                                                  Environment.StackTrace);
      }
    }

    public MobilePushShopperAddResponseData(MobilePushShopperAddRequestData requestData, Exception ex)
    {
      Xml = string.Empty;
      AtlantisException = new AtlantisException(requestData,
                                                MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                ex.Message,
                                                ex.StackTrace);
    }

    private bool ParseResponseXml(string xml)
    {
      bool parseSuccess = false;

      if (!string.IsNullOrEmpty(xml))
      {
        try
        {
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.LoadXml(xml);
          XmlNode statusNode = xmlDocument.SelectSingleNode("//Status");
          if(statusNode != null)
          {
            IsSuccess = string.Compare(statusNode.InnerText, "SUCCESS", true) == 0;
            parseSuccess = true;  
          }
        }
        catch
        {
          parseSuccess = false;
        }
      }

      return parseSuccess;
    }

    public string ToXML()
    {
      return Xml;
    }

    public AtlantisException GetException()
    {
      return AtlantisException;
    }
  }
}