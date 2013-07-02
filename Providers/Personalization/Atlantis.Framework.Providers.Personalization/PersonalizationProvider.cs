using Atlantis.Framework.Interface;
using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Personalization
{
  public class PersonalizationProvider : ProviderBase, IPersonalizationProvider
  {

    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<IShopperContext> _shopperContext;
    //private readonly Lazy<ICurrencyProvider> _currency;
    //private readonly Lazy<ILocalizationProvider> _localization;

    public PersonalizationProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
    }

    public TargetedMessages GetTargetedMessages()
    {
      string appId = "2";
      string interactionPoint = "Homepage";
      bool shopperTrusted = (_shopperContext.Value.ShopperStatus == ShopperStatusType.PartiallyTrusted) || (_shopperContext.Value.ShopperStatus == ShopperStatusType.Authenticated);

      Dictionary<string, string> contextData = new Dictionary<string, string>();
      contextData.Add("TransactionalCurrency", "USD");
      contextData.Add("Language", "en");

      Dictionary<string, string> shopperData = new Dictionary<string, string>();
      shopperData.Add("PrivateLabelId", _siteContext.Value.PrivateLabelId.ToString());
      shopperData.Add("ShopperId", _shopperContext.Value.ShopperId);
      shopperData.Add("IsShopperAuthenticated", shopperTrusted ? "1" : "0");

      RequestData request = new TargetedMessagesRequestData(_shopperContext.Value.ShopperId, String.Empty, String.Empty, _siteContext.Value.Pathway,
        _siteContext.Value.PageCount, appId, interactionPoint, contextData, shopperData);

      TargetedMessagesResponseData response = SessionCache.SessionCache.GetProcessRequest<TargetedMessagesResponseData>(request, PersonalizationEngineRequests.RequestId);

      return response.TargetedMessagesData;
    }
  }
}
