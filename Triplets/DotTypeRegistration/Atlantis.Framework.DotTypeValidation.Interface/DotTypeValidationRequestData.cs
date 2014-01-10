using System.Collections.Generic;
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
    public string Category { get; set; }

    public Dictionary<string, IDotTypeValidationFieldValueData> Fields = new Dictionary<string, IDotTypeValidationFieldValueData>();

    public DotTypeValidationRequestData(string clientApplication, string serverName, int tldId, string phase, string category,
                                        Dictionary<string, IDotTypeValidationFieldValueData> fields)
    {
      ClientApplication = clientApplication;
      ServerName = serverName;
      TldId = tldId;
      Phase = phase;
      Category = category;
      Fields = fields;
    }

    public override string ToXML()
    {
      var rootElement = new XElement("application");
      rootElement.Add(new XAttribute("clientapplication", ClientApplication));
      rootElement.Add(new XAttribute("servername", ServerName));
      rootElement.Add(new XAttribute("tldid", TldId));
      rootElement.Add(new XAttribute("phase", Phase));
      rootElement.Add(new XAttribute("category", Category));

      if (Fields.Count > 0)
      {
        foreach (var field in Fields)
        {
          if (!string.IsNullOrEmpty(field.Key) && field.Value != null)
          {
            var fieldElement = new XElement("key");
            fieldElement.Add(new XAttribute("name", field.Key));
            fieldElement.Add(new XAttribute("value", field.Value.Value));

            if (field.Value.NoValidate)
            {
              fieldElement.Add(new XAttribute("novalidate", field.Value.NoValidate));
            }
            rootElement.Add(fieldElement);
          }
        }
      }

      return rootElement.ToString();
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(ToXML());
    }
  }
}
