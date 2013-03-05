using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.AppSettings.Interface
{
  public class AppSettingResponseData : IResponseData
  {
    private static AppSettingResponseData _emptySettingValue;

    static AppSettingResponseData()
    {
      _emptySettingValue = new AppSettingResponseData(string.Empty);
    }

    public static AppSettingResponseData EmptySetting
    {
      get { return _emptySettingValue; }
    }

    public static AppSettingResponseData FromSettingValue(string settingValue)
    {
      if (string.IsNullOrEmpty(settingValue))
      {
        return _emptySettingValue;
      }
      else
      {
        return new AppSettingResponseData(settingValue);
      }
    }

    public static AppSettingResponseData FromException(AtlantisException ex)
    {
      return new AppSettingResponseData(ex);
    }

    private AtlantisException _exception = null;

    private AppSettingResponseData(string settingValue)
    {
      SettingValue = settingValue;
    }

    private AppSettingResponseData(AtlantisException ex)
    {
      _exception = ex;
      SettingValue = string.Empty;
    }

    public string SettingValue { get; private set; }

    public string ToXML()
    {
      XElement element = new XElement("AppSetting");
      element.Add(new XAttribute("value", SettingValue));
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
