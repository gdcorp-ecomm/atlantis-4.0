using System.Xml.Serialization;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  [XmlRoot(ElementName = "Data")]
  public class PlaceHolderData
  {
    [XmlAttribute(AttributeName = "location")]
    public string Location { get; set; }

    [XmlArray(ElementName = "Parameters")]
    [XmlArrayItem(ElementName = "Parameter")]
    public PlaceHolderParameter[] Parameters { get; set; }
  }
}
