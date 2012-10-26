using System.Collections.Generic;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;
using Atlantis.Framework.CDS.Entities.Widgets;

namespace Atlantis.Framework.CDS.Entities.SalesTables.Widgets
{
  public class ComparePlansTable : IWidgetModel, IAddToCartWidgetModel
  {

    public string CiCode { get; set; }
    public string CrossSellConfig { get; set; }
    public string FormName { get; set; }
    public string FormActionUrl { get; set; }
    public bool FromDPP { get; set; }
    public string ISC { get; set; }
    public string ItemTrackingCode { get; set; }
    public bool RedirectStraightToCart { get; set; }
    public bool SkipDomainerCheck { get; set; }
    public string Upsell { get; set; }
    public bool UseNewCrossSellPage { get; set; }
    public string XS_AppSettingKey { get; set; }
    public string XS_ContainerCode { get; set; }
    public string XS_LaunchCICode { get; set; }

    public int NumberOfColumns { get; set; }

    public List<ComparePlansTableColumn> Columns { get; set; }
    public List<ComparePlansTableRow> Rows { get; set; }
    public string JavaScript { get; set; }

    public class ComparePlansTableRow : TldElementBase
    {
      public bool IsBold { get; set; }
      public bool IsSubheading { get; set; }
      public List<string> Row { get; set; }
    }

    public class ComparePlansTableColumn : TldElementBase
    {
      public ComparePlansTableColumn()
      {
        this.ProductOption = new ProductOption();
      }

      public int Pfid { get; set; }
      public bool AddToCartButton { get; set; }
      public bool OrangeAddToCartButton { get; set; }
      public ProductOption ProductOption { get; set; }

      public string Heading { get; set; }
      public string Styles { get; set; }
    }
  }
}
