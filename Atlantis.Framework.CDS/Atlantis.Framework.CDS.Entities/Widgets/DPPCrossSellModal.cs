using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class DPPCrossSellModal : IWidgetModel
  {
    public DPPCrossSellModal()
    {
    }

    public string Title { get; set; }
    public string ButtonTextNoSelection { get; set; }
    public string ButtonTextSelected { get; set; }
    public List<GroupedProduct> GroupedProducts { get; set; }
    public string DisclaimerText { get; set; }
    public string DisclaimerTitle { get; set; }

    public class GroupedProduct
    {
      public string Title { get; set; }
      public List<Product> Products { get; set; } // Needs to be filterable by if the product group is offered
      public List<AltProduct> AltProducts { get; set; } // Needs to be filterable by if the product group is offered and datacenter

      public class Product : ElementBase
      {
        public string Title { get; set; }
        public string Description { get; set; }
        public string AltText { get; set; }
        public string ProductSpriteImage { get; set; }
        public string SpritePosition { get; set; }
        public List<SubProduct> SubProducts { get; set; }  // Needs to be filterable and changed based on datacenter
        public List<PopIn> PopIns { get; set; }
        public List<Disclaimer> Disclaimers { get; set; }

        public class SubProduct : ElementBase
        {
          public string ProductID { get; set; }
          public string Title { get; set; }
          public string Price { get; set; }
          public string ListPrice { get; set; }
          public string SelectedText { get; set; }
          public List<string> Features { get; set; }
        }
      }

      public class AltProduct : ElementBase
      {
        public string ProductID { get; set; }
        public PopIn Details { get; set; }
        public List<Disclaimer> Disclaimers { get; set; }
      }

    }

    // Possibly this could be added to PageSharedData via a Plugin or previous widget - need more details.
    public class Disclaimer : ElementBase  // Needs to be filterable based on whether or not the symbols are added.  IE, if hosting is not displayed, don't display the hosting products  
    {
      public Disclaimer(string symbol, string text)
      {
        Symbol = symbol;
        Text = text;
      }

      public string Symbol { get; set; }
    }

    public class PopIn
    {
      public PopIn(string title, string text)
      {
        Title = title;
        Text = text;
      }

      public string Title { get; set; }
      public string Text { get; set; }

      //TODO: Should be in code-behind of widget:
      public string Display
      {
        get
        {
          return string.Format("<span class=\"g-hover\">{0} <div class=\"g-hover-bubble\" style=\"display:none;\">{1}</div></span>", Title, Text);
        }
      }
    }
  }
}