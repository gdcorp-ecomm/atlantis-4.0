using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddBasketAttribute.Interface
{
  public class AddBasketAttributeRequestData : RequestData
  {
    private readonly Dictionary<string, string> _basketAttributes = new Dictionary<string,string>();

    public string BasketType { get; set; }
    
    public AddBasketAttributeRequestData(string shopperId,
								                         string sourceUrl,
								                         string orderId,
								                         string pathway,
								                         int pageCount,
                                         string basketType) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
		{
      BasketType = basketType;
      RequestTimeout = TimeSpan.FromSeconds(12);
		}

    public void AddAttribute(string name, string value)
    {
      if (name != null)
      {
        _basketAttributes[name] = value;
      }
    }

    public void AddAttributes(IEnumerable<KeyValuePair<string, string>> nameValues)
    {
      if (nameValues != null)
      {
        foreach (KeyValuePair<string, string> nameValue in nameValues)
        {
          _basketAttributes[nameValue.Key] = nameValue.Value;
        }
      }
    }

    public void AddAttributes(IEnumerable<KeyValuePair<string, string>> nameValues, params string[] limitToTheseKeysOnly)
    {
      if ((nameValues != null) && (limitToTheseKeysOnly != null))
      {
        HashSet<string> validKeys = new HashSet<string>(limitToTheseKeysOnly, StringComparer.InvariantCultureIgnoreCase);
        foreach (KeyValuePair<string, string> nameValue in nameValues)
        {
          if (validKeys.Contains(nameValue.Key))
          {
            _basketAttributes[nameValue.Key] = nameValue.Value;
          }
        }
      }
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("AddBasketAttrib is not cacheable.");
    }

    private static void AddFieldElement(XmlTextWriter xmlWriter, KeyValuePair<string, string> nameValuePair)
    {
      if ((!string.IsNullOrEmpty(nameValuePair.Key)) && (nameValuePair.Value != null))
      {
        xmlWriter.WriteStartElement("item");
        xmlWriter.WriteAttributeString("name", nameValuePair.Key);
        xmlWriter.WriteValue(nameValuePair.Value);
        xmlWriter.WriteEndElement();
      }
    }

    public override string ToXML()
    {
      StringBuilder result = new StringBuilder();
      using (XmlTextWriter xmlWriter = new XmlTextWriter(new StringWriter(result)))
      {
        xmlWriter.WriteStartElement("requestInfo");
        xmlWriter.WriteStartElement("dictionary");

        foreach (KeyValuePair<string, string> field in _basketAttributes)
        {
          AddFieldElement(xmlWriter, field);
        }
        
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
      }

      return result.ToString();
    }
  }
}
