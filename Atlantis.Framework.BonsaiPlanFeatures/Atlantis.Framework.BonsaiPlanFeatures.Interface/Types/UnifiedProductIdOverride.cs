using System;
using System.Xml.Serialization;

namespace Atlantis.Framework.BonsaiPlanFeatures.Interface.Types
{
  [Serializable]
  [XmlRoot(ElementName = "Override")]
  public class UnifiedProductIdOverride
  {
    [XmlAttribute(AttributeName = "UnifiedProductID")]
    public int UnifiedProductId { get; set; }

    [XmlAttribute]
    public int Quantity { get; set; }
    
    public UnifiedProductIdOverride() { }
  }
}
