using System;
using System.Xml.Serialization;

namespace Atlantis.Framework.FaxEmailApplyAddonPack.Impl.Types
{
  [Serializable]
  public sealed class Notes
  {
    [XmlElement("SHOPPERNOTE")]
    public ShopperNote ShopperNote { get; set; }

    [XmlElement("ACTIONNOTE")]
    public ActionNote ActionNote { get; set; }
  }
}
