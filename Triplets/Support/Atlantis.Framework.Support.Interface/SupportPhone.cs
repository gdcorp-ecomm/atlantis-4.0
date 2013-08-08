namespace Atlantis.Framework.Support.Interface
{
  public class SupportPhone
  {
    public string TechnicalSupportPhone { get; private set; }
    public bool SupportPhoneIsInternational { get; private set; }

    public SupportPhone(string technicalSupportPhone, bool supportPhoneIsInternational)
    {
      TechnicalSupportPhone = technicalSupportPhone;
      SupportPhoneIsInternational = supportPhoneIsInternational;
    }
  }
}
