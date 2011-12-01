using System;
using System.Xml.Serialization;

namespace Atlantis.Framework.FaxEmailApplyAddonPack.Impl.Types
{
  [Serializable]
  [XmlRoot("ACTIONROOT")]
  public sealed class ActionRoot
  {
    [XmlElement("ACTION")]
    public Action Action { get; set; }

    [XmlElement("FAXEMAIL")]
    public FaxEmail FaxEmail { get; set; }

    [XmlElement("NOTES")]
    public Notes Notes { get; set; }
  }
}
