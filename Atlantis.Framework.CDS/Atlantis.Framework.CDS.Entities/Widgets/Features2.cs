using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Filters;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class Features2 : IWidgetModel
  {
    public Features2()
    {
    }

    public string FeatureSpriteImage { get; set; }
    public int FeatureWidth { get; set; }

    private int _numColumns = 1;
    public int NumColumns
    {
      get { return _numColumns; }
      set { _numColumns = value; }
    }

    public class Feature: FilterBase
    {
      public Feature()
      {
        Title = string.Empty;
        Text = string.Empty;
        SpritePosition = string.Empty;
        SpriteHeight = string.Empty;
      }

      public string Text { get; set; }
      public string Title { get; set; }
      public string SpritePosition { get; set; }
      public string SpriteHeight { get; set; }

      public class FeatureListItem : ElementBase
      {
      }

      public List<FeatureListItem> FeatureListItems { get; set; }
    }

    public List<Feature> CurrentFeatures { get; set; }
  }
}
