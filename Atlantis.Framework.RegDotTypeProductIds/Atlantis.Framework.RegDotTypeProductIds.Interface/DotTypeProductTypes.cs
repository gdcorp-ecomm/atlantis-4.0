using System.ComponentModel;

namespace Atlantis.Framework.RegDotTypeProductIds.Interface
{
  public enum DotTypeProductTypes
  {
    None,
    Registration,
    Transfer,
    Renewal,
    PreRegister,
    ExpiredAuctionReg,
    [Description("Sunrise A")]
    SunriseA,
    [Description("Sunrise B")]
    SunriseB,
    [Description("Sunrise C")]
    SunriseC,
    Landrush,
    [Description("General Availability")]
    GeneralAvailability,
    [Description("Sunrise A Application")]
    SunriseAApplication,
    [Description("Sunrise B Application")]
    SunriseBApplication,
    [Description("Sunrise C Application")]
    SunriseCApplication,
    [Description("Landrush Application")]
    LandrushApplication,
    Trustee
  }
}
