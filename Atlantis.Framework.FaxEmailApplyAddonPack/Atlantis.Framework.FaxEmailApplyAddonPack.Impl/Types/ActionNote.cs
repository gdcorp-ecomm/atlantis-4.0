using System;
using System.Xml.Serialization;

namespace Atlantis.Framework.FaxEmailApplyAddonPack.Impl.Types
{
  [Serializable]
  public sealed class ActionNote
  {
    [XmlAttribute("note")]
    public string Note { get; set; }

    [XmlAttribute("modifiedby")]
    public string ModifiedBy { get; set; }
  }
}
