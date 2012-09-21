using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class DPPPrivacyModal : IWidgetModel
  {
    public DPPPrivacyModal()
    {
    }
    public string Title { get; set; }
    public string ButtonTextNoSelection { get; set; }
    public string ButtonTextSelected { get; set; }
    public string DisclaimerText { get; set; }
    public string DisclaimerTitle { get; set; }
    public List<GroupedProduct> GroupedProducts { get; set; }
    public class GroupedProduct : ElementBase
    {
      public string Title { get; set; }
      public List<Product> Products { get; set; }

      public class Product : ElementBase
      {
        public string ProductGroupID { get; set; }
        public string CICode { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string WhyInfoText { get; set; }
        public string CompareInfoText { get; set; }
        public WhyInfo WhyInfo { get; set; }
        public CompareInfo CompareInfo { get; set; }
        public string DBPText { get; set; }
        public string ProductSpriteImage { get; set; }
        public string SpritePosition { get; set; }
        public List<SubProduct> SubProducts { get; set; }
        public List<PopIn> PopIns { get; set; }
        public List<Disclaimer> Disclaimers { get; set; }

        public class SubProduct
        {
          public string ProductID { get; set; }
          public string Title { get; set; }
          public string Price { get; set; }
          public string ListPrice { get; set; }
          public string SelectedText { get; set; }
          public List<string> Features { get; set; }
        }
      }
    }

    public class WhyInfo
    {
      public WhyInfo()
      {
      }

      public string Description { get; set; }
      public List<RegistrationOptionsInfo> RegistrationOptions { get; set; }

      public class RegistrationOptionsInfo
      {
        public string Title { get; set; }
        public string AltText { get; set; }
        public string Description { get; set; }
      }
    }

    public class CompareInfo
    {
      public CompareInfo()
      { }

      public string Description { get; set; }
      public List<Product> Products { get; set; }
      public List<Feature> Features { get; set; }

      public class Product
      {
        public string Title { get; set; }
        public string SubTitle { get; set; }
      }

      public class Feature
      {
        public string Description { get; set; }
        public List<bool> ProductsUsed { get; set; }
      }
    }

    public class Disclaimer : ElementBase  // Needs to be filterable based on whether or not the symbols are added.  IE, if hosting is not displayed, don't display the hosting products  
    {
      public Disclaimer() { }

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

    }
  }
}