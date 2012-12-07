using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects
{
  public class Pixel
  {
    public string Value { get; set; }
    public string Name { get; set; }
    public string PixelType { get; set; }
    public List<string> CiCodes { get; set; }
    public string AppSettingName { get; set; }

    public Pixel(string value, string name, string type, List<string> ciCodes, string appSetting = "")
    {
      Value = Decode(value);
      PixelType = type;
      CiCodes = ciCodes;
      Name = name;
      AppSettingName = appSetting;
    }

    private string Decode(string deserializeString)
    {
      return System.Web.HttpUtility.HtmlDecode(deserializeString);
    }
  }
}
