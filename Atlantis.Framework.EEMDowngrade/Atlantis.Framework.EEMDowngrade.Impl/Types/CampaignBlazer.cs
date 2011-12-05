using System;
using System.Xml.Serialization;

namespace Atlantis.Framework.EEMDowngrade.Impl.Types
{
  [Serializable]
  public sealed class CampaignBlazer
  {
    [XmlAttribute("new_pfid")]
    public int DowngradeProductId { get; set; }
  }
}
