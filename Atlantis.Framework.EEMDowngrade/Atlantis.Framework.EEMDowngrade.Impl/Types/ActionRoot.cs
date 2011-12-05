using System;
using System.Xml.Serialization;

namespace Atlantis.Framework.EEMDowngrade.Impl.Types
{
  [Serializable]
  [XmlRoot("ACTIONROOT")]
  public sealed class ActionRoot
  {
    [XmlElement("ACTION")]
    public Action Action { get; set; }

    [XmlElement("CAMPAIGNBLAZER")]
    public CampaignBlazer CampaignBlazer { get; set; }

    [XmlElement("NOTES")]
    public Notes Notes { get; set; }
  }
}
