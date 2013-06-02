using Atlantis.Framework.AppSettings.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Currency.Tests.Mocks
{
  public class MockAppSettingRequest : IRequest
  {
    const string _ISCPRICINGACTIVESETTING = "ATLANTIS_PRICING_ISC_ACTIVE";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AppSettingRequestData request = requestData as AppSettingRequestData;

      if (request.SettingName == _ISCPRICINGACTIVESETTING)
      {
        return HandleIscPricingProviderSetting();
      }

      return AppSettingResponseData.EmptySetting;
    }

    #endregion

    private string _iscPricingProviderActive;
    public string IscPricingProviderActive
    {
      get { return _iscPricingProviderActive; }
      set { _iscPricingProviderActive = value; }
    }

    private IResponseData HandleIscPricingProviderSetting()
    {
      AppSettingResponseData result = AppSettingResponseData.EmptySetting;
      if (!string.IsNullOrWhiteSpace(_iscPricingProviderActive))
      {
        result = AppSettingResponseData.FromSettingValue(_iscPricingProviderActive);
      }

      return result;
    }
  }
}
