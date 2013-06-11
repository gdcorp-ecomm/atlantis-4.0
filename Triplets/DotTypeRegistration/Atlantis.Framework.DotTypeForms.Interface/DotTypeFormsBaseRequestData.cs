using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public abstract class DotTypeFormsBaseRequestData : RequestData
  {
    int _tldId { get; set; }
    string _placement { get; set; }
    string _phase { get; set; }
    string _language { get; set; }

    protected DotTypeFormsBaseRequestData(int tldId, string placement, string phase, string language)
    {
      _tldId = tldId;
      _placement = placement;
      _phase = phase;
      _language = language;
    }

    public override string ToXML()
    {
      var element = new XElement("parameters");
      element.Add(new XAttribute("tldid", _tldId.ToString(CultureInfo.InvariantCulture)));
      element.Add(new XAttribute("placement", _placement));
      element.Add(new XAttribute("phase", _phase));
      element.Add(new XAttribute("language", _language));
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
