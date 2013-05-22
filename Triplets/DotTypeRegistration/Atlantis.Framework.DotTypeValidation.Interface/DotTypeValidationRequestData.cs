using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeValidation.Interface
{
  public class DotTypeValidationRequestData : RequestData
  {
    public string ClientApplication { get; set; }
    public string ServerName { get; set; }
    public int TldId { get; set; }
    public string Phase { get; set; }
    public Dictionary<string, string> Fields = new Dictionary<string,string>();

    public DotTypeValidationRequestData(string clientApplication, string serverName, int tldId, string phase, Dictionary<string, string> fields)
    {
      ClientApplication = clientApplication;
      ServerName = serverName;
      TldId = tldId;
      Phase = phase;
      Fields = fields;
    }

    public override string ToXML()
    {
      var rootElement = new XElement("application");
      rootElement.Add(new XAttribute("clientapplication", ClientApplication));
      rootElement.Add(new XAttribute("servername", ServerName));
      rootElement.Add(new XAttribute("tldid", TldId));
      rootElement.Add(new XAttribute("phase", Phase));

      foreach (var field in Fields)
      {
        var fieldElement = new XElement("key");
        fieldElement.Add(new XAttribute("name", field.Key));
        fieldElement.Add(new XAttribute("value", field.Value));
        rootElement.Add(fieldElement);
      }

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
