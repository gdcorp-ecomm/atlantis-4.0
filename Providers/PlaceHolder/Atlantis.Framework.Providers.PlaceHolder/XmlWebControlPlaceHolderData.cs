using Atlantis.Framework.Providers.PlaceHolder.Interface;
using System.Xml.Serialization;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  [XmlRoot(ElementName = "Data")]
  public class XmlWebControlPlaceHolderData : XmlPlaceHolderData, IWebControlPlaceHolderData
  {
    [XmlAttribute(AttributeName = "assemblyname")]
    public string AssemblyName { get; set; }
  }
}
