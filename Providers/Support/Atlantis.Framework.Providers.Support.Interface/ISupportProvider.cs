using Atlantis.Framework.Support.Interface;

namespace Atlantis.Framework.Providers.Support.Interface
{
  public interface ISupportProvider
  {
    ISupportPhoneData GetSupportPhone(SupportPhoneType supportPhoneType);
    string SupportEmail { get; }
  }
}
