using System;
using System.Xml.Serialization;

namespace Atlantis.Framework.FaxEmailApplyAddonPack.Impl.Types
{
  [Serializable]
  public sealed class FaxEmail
  {
    [XmlAttribute("child_resource_id")]
    public int ChildResourceId { get; set; }

    [XmlAttribute("external_resource_id")]
    public string ExternalResourceId { get; set; }

    [XmlAttribute("child_external_resource_id")]
    public string ChildExternalResourceId { get; set; }
  }
}
