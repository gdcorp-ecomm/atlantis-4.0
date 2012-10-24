
namespace Atlantis.Framework.CDS.Entities.Common.Interfaces
{
  public interface IAddToCartWidgetModel : IWidgetModel
  {
    string CiCode { get; set; }
    string CrossSellConfig { get; set; }
    string FormName { get; set; }
    string FormActionUrl { get; set; }
    bool FromDPP { get; set; }
    string ISC { get; set; }
    string ItemTrackingCode { get; set; }
    bool RedirectStraightToCart { get; set; }
    bool SkipDomainerCheck { get; set; }
    string Upsell { get; set; }
    bool UseNewCrossSellPage { get; set; }
    string XS_AppSettingKey { get; set; }
    string XS_ContainerCode { get; set; }
    string XS_LaunchCICode { get; set; }
  }
}

