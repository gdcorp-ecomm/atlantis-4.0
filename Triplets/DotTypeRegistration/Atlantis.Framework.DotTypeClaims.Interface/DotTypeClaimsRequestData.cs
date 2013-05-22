using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeClaims.Interface
{
  public class DotTypeClaimsRequestData : RequestData
  {
    public int TldId { get; set; }

    public DotTypeClaimsRequestData(int tldId)
    {
      TldId = tldId;
    }

    public override string ToXML()
    {
      var element = new XElement("DotTypeClaimsRequestData");
      element.Add(new XAttribute("tldid", TldId.ToString(CultureInfo.InvariantCulture)));
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
