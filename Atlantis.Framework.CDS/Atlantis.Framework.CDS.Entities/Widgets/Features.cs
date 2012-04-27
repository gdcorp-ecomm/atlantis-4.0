using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;
using Atlantis.Framework.CDS.Entities.Attributes;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class Features : IWidgetModel
  {
    public Features()
    {
    }

    public string FeatureSpriteImage { get; set; }
    public int RightBoxFeatureWidth { get; set; }
    [HideInManagementUI]
    public bool Filtered { get; set; }

    public class Feature: ElementBase
    {
      public Feature()
      {
        Title = string.Empty;
        Text = string.Empty;
        SpritePosition = string.Empty;
      }

      public string Title { get; set; }
      public string SpritePosition { get; set; }
      [HideInManagementUI]
      public bool Filtered { get; set; }

      public class FeatureListItem : ElementBase
      {
      }

      public List<FeatureListItem> FeatureListItems { get; set; }
    }

    public List<Feature> CurrentFeatures { get; set; }
  }
}
