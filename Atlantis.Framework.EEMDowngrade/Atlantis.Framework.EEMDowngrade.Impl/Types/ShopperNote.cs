using System;
using System.Xml.Serialization;

namespace Atlantis.Framework.EEMDowngrade.Impl.Types
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
