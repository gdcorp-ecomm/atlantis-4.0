using System;
using System.Xml.Serialization;

namespace Atlantis.Framework.FaxEmailApplyAddonPack.Impl.Types
{
  [Serializable]
  public sealed class ShopperNote
  {
    [XmlAttribute("note")]
    public string Note { get; set; }

    [XmlAttribute("enteredby")]
    public string EnteredBy { get; set; }
  }
}
