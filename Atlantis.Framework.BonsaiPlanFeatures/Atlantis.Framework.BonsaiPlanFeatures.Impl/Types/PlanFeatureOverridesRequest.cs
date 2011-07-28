using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Atlantis.Framework.BonsaiPlanFeatures.Interface.Types;

namespace Atlantis.Framework.BonsaiPlanFeatures.Impl.Types
{
  [Serializable]
  [XmlRoot(ElementName = "BaseUnifiedProductID")]
  public class PlanFeatureOverridesRequest
  {
    [XmlAttribute(AttributeName = "UnifiedProductID")]
    public int UnifiedProductId { get; set; }

    [XmlAttribute]
    public int IsFree { get; set; }

    [XmlArrayAttribute(ElementName = "UnifiedProductIDOverrides", IsNullable = false)]
    [XmlArrayItem(ElementName = "Override")]
    public List<UnifiedProductIdOverride> UnifiedProductIdOverrides { get; set; }

    public PlanFeatureOverridesRequest() { }
  }
}
