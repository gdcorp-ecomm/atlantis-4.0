using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobilePushShopperGet.Interface
{
  public class MobilePushShopperGetResponseData : IResponseData
  {
    private AtlantisException AtlantisException { get; set; }

    public string Xml { get; set; }

    private readonly IList<MobilePushShopperRecord> _records = new List<MobilePushShopperRecord>(16);
    public IList<MobilePushShopperRecord> Records
    {
      get { return _records; }
    }

    public bool IsSuccess { get; private set; }

    public MobilePushShopperGetResponseData(MobilePushShopperGetRequestData requestData, string responseXml)
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

    public MobilePushShopperGetResponseData(MobilePushShopperGetRequestData requestData, Exception ex)
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

          XmlNodeList itemNodes = xmlDocument.SelectNodes("//item");
          if(itemNodes != null)
          {
            foreach (XmlNode itemNode in itemNodes)
            {
              if(itemNode.Attributes != null)
              {
                int shopperPushId = 0;
                XmlAttribute shopperPushIdAttribute = itemNode.Attributes["shopperMobilePushNotificationID"];
                if(shopperPushIdAttribute != null)
                {
                  int.TryParse(shopperPushIdAttribute.Value, out shopperPushId);
                }

                string registrationId = string.Empty;
                XmlAttribute registrationIdAttribute = itemNode.Attributes["registrationID"];
                if(registrationIdAttribute != null)
                {
                  registrationId = registrationIdAttribute.Value;
                }


                string mobileAppId = string.Empty;
                XmlAttribute mobileAppIdAttribute = itemNode.Attributes["fortKnox_shopperMobileAppID"];
                if(mobileAppIdAttribute != null)
                {
                  mobileAppId = mobileAppIdAttribute.Value;
                }

                string mobileDeviceId = string.Empty;
                XmlAttribute deviceIdAttribute = itemNode.Attributes["deviceID"];
                if (deviceIdAttribute != null)
                {
                  mobileDeviceId = deviceIdAttribute.Value;
                }

                Records.Add(new MobilePushShopperRecord(shopperPushId, registrationId, mobileAppId, mobileDeviceId));
              }
            }
          }
        }
        catch
        {
          parseSuccess = false;
        }
      }

      return parseSuccess;
    }

    public bool HasPushEnabled(string registrationId)
    {
      bool hasPushEnabled = false;
      foreach (MobilePushShopperRecord record in Records)
      {
        if (record.RegistrationId == registrationId)
        {
          hasPushEnabled = true;
          break;
        }
      }
      return hasPushEnabled;
    }

    public bool HasPushEnabledForDevice(string mobileDeviceId)
    {
      bool hasPushEnabled = false;
      foreach (MobilePushShopperRecord record in Records)
      {
        if (record.MobileDeviceId == mobileDeviceId)
        {
          hasPushEnabled = true;
          break;
        }
      }
      return hasPushEnabled;
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
