using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects
{
  public class Pixel
  {
    private IEnumerable<CookieData> _cookieValues;

    public string Value { get; set; }
    public string Name { get; set; }
    public string PixelType { get; set; }
    public List<string> CiCodes { get; set; }
    public string AppSettingName { get; set; }
    public string TriggerReturn { get; set; }

    public Pixel(string value, string name, string type, List<string> ciCodes, IEnumerable<CookieData> cookieValues, string appSetting = "")
    {
      Value = Decode(value);
      PixelType = type;
      CiCodes = ciCodes;
      Name = name;
      AppSettingName = appSetting;
      this.Cookies = cookieValues;
    }

    private string Decode(string deserializeString)
    {
      return System.Web.HttpUtility.HtmlDecode(deserializeString);
    }

    public IEnumerable<CookieData> Cookies
    {
      get
      {
        return _cookieValues ?? (_cookieValues = Enumerable.Empty<CookieData>());
      }
      private set
      {
        _cookieValues = value;
      }
    }
  }
}
