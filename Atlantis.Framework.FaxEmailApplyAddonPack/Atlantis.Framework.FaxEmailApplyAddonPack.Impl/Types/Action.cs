using System;
using System.Xml.Serialization;

namespace Atlantis.Framework.FaxEmailApplyAddonPack.Impl.Types
{
  [Serializable]
  public sealed class Action
  {
    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlAttribute("privatelabelid")]
    public int PrivateLabelId { get; set; }

    [XmlAttribute("shopper_id")]
    public string ShopperId { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; }
  }
}
