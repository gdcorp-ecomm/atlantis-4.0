using Atlantis.Framework.Providers.DomainContactValidation.Interface;

namespace Atlantis.Framework.Providers.DomainContactValidation
{
  public class TuiFormInfo : ITuiFormInfo
  {
    public string TuiFormType { get; set; }
    public string VendorId { get; set; }

    public TuiFormInfo(string tuiFormType, string vendorId)
    {
      TuiFormType = tuiFormType;
      VendorId = vendorId;
    }
  }
}
