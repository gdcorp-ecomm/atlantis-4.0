using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class GeneralEoiJsonRequestData : RequestData
  {
    public string LanguageCode { get; set; }

    public GeneralEoiJsonRequestData(string languageCode)
    {
      LanguageCode = languageCode;

      RequestTimeout = TimeSpan.FromSeconds(10);
    }

    public override string ToXML()
    {
      var element = new XElement("GeneralEoiJsonRequestData");
      element.Add(new XAttribute("languageCode", LanguageCode));
      return element.ToString();
    }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();
      var requestXml = ToXML();
      var data = Encoding.UTF8.GetBytes(requestXml);
      var hash = md5.ComputeHash(data);
      var result = Encoding.UTF8.GetString(hash);
      return result;
    }
  }
}
