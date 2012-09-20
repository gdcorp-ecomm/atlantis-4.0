using System.Xml;
using System.Text;
using System.IO;

namespace Atlantis.Framework.GetBasketObjects.Interface
{
  public class CartDomainEntry : CartBaseDictionary
  {
    public string SecondLevelDomain
    {
      get { return GetStringProperty("sld", string.Empty); }
    }

    public string TopLevelDomain
    {
      get { return GetStringProperty("tld", string.Empty); }
    }

    public string Duration
    {
      get { return GetStringProperty("duration", string.Empty); }
    }

    public string Period
    {
      get { return GetStringProperty("period", string.Empty); }
    }

    public string ResourceId
    {
      get { return GetStringProperty("resourceid", string.Empty); }
    }

    public void AddToXml(XmlTextWriter writer)
    {
      writer.WriteStartElement("domain");
      writer.WriteAttributeString("sld", SecondLevelDomain);
      writer.WriteAttributeString("tld", TopLevelDomain);
      writer.WriteAttributeString("duration", Duration);
      if (!string.IsNullOrEmpty(ResourceId))
      {
        writer.WriteAttributeString("resourceid", ResourceId);  
      }
      writer.WriteEndElement();
    }

    public string ToXML()
    {
      StringBuilder customXML = new StringBuilder(100);
      XmlTextWriter customXmlWriter = new XmlTextWriter(new StringWriter(customXML));
      AddToXml(customXmlWriter);
      return customXML.ToString();
    }

    public CartDomainEntry()
    {
    }

    public CartDomainEntry(string sld, string tld, string duration)
    {
      Add("sld", sld);
      Add("tld", tld);
      Add("duration", duration);
    }
  }
}
