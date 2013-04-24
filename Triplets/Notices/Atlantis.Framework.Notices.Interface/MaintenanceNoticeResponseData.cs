using Atlantis.Framework.Interface;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.Notices.Interface
{
  public class MaintenanceNoticeResponseData : IResponseData
  {
    public static MaintenanceNoticeResponseData NoNotice { get; private set; }

    static MaintenanceNoticeResponseData()
    {
      NoNotice = new MaintenanceNoticeResponseData(false, string.Empty, string.Empty);
    }
    
    public static MaintenanceNoticeResponseData FromCacheDataXml(string cacheDataXml)
    {
      MaintenanceNoticeResponseData result = NoNotice;

      if (!string.IsNullOrEmpty(cacheDataXml))
      {
        try
        {
          //<data count="1"><item maintID="46" maintHeader="IDP - new notice" maintText="This is a notice just for IDP." maintOn="1" /></data>
          XElement dataElement = XElement.Parse(cacheDataXml);
          XElement itemElement = dataElement.Descendants("item").FirstOrDefault();
          if (itemElement != null)
          {
            var onAtt = itemElement.Attribute("maintOn");
            var headerAtt = itemElement.Attribute("maintHeader");
            var bodyAtt = itemElement.Attribute("maintText");

            if (onAtt.Value == "1")
            {
              result = new MaintenanceNoticeResponseData(true, headerAtt.Value, bodyAtt.Value);
            }
          }
        }
        catch (Exception ex)
        {
          AtlantisException exception = new AtlantisException("MaintenanceNoticeResponseData.FromCacheDataXml", "0", ex.Message + ex.StackTrace, cacheDataXml, null, null);
          Engine.Engine.LogAtlantisException(exception);
          result = NoNotice;
        }
      }

      return result;
    }

    private MaintenanceNoticeResponseData(bool isNoticeOn, string noticeHeader, string noticeBody)
    {
      IsNoticeOn = isNoticeOn;
      NoticeHeader = noticeHeader ?? string.Empty;
      NoticeBody = noticeBody ?? string.Empty;
    }

    public bool IsNoticeOn { get; private set; }
    public string NoticeHeader { get; private set; }
    public string NoticeBody { get; private set; }

    public string ToXML()
    {
      XElement element = new XElement("maintenancenotice");
      element.Add(
        new XAttribute("isnoticeon", IsNoticeOn.ToString()),
        new XAttribute("noticeheader", NoticeHeader),
        new XAttribute("noticebody", NoticeBody));

      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
