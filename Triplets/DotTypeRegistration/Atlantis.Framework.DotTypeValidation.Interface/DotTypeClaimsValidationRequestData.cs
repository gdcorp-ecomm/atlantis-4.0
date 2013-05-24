using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeValidation.Interface
{
  public class DotTypeClaimsValidationRequestData : RequestData
  {
    public string ClientApplication { get; set; }
    public string ServerName { get; set; }
    public int TldId { get; set; }
    public string Phase { get; set; }

    public DotTypeClaimsValidationRequestData(string clientApplication, string serverName, int tldId, string phase/* Claims notice object*/)
    {
      ClientApplication = clientApplication;
      ServerName = serverName;
      TldId = tldId;
      Phase = phase;
    }

    public override string ToXML()
    {
      var rootElement = new XElement("application");
      rootElement.Add(new XAttribute("clientapplication", ClientApplication));
      rootElement.Add(new XAttribute("servername", ServerName));
      rootElement.Add(new XAttribute("tldid", TldId));
      rootElement.Add(new XAttribute("phase", Phase));

      //Convert Claims Notice object to xml

      return rootElement.ToString();
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
