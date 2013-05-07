using System.Xml.Serialization;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  [XmlRoot(ElementName = "Parameter")]
  public class PlaceHolderParameter
  {
    [XmlAttribute(AttributeName = "key")]
    public string Key { get; set; }

    [XmlAttribute(AttributeName = "value")]
    public string Value { get; set; }
  }
}
