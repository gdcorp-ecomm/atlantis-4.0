using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.AppSettings.Interface
{
  public class AppSettingRequestData : RequestData
  {
    public const string InvalidSettingName = "{INVALIDSETTINGNAME}";

    public string SettingName { get; private set; }

    public AppSettingRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string settingName)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      if (!string.IsNullOrEmpty(settingName))
      {
        SettingName = settingName;
      }
      else
      {
        SettingName = InvalidSettingName;
      }
    }

    public override string GetCacheMD5()
    {
      return SettingName.ToUpperInvariant();
    }

    public override string ToXML()
    {
      XElement element = new XElement("AppSettingRequestData");
      element.Add(new XAttribute("name", SettingName));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
