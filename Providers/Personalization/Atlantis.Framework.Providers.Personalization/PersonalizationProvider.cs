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

    public PersonalizationProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
    }

    public TargetedMessages GetTargetedMessages(string appId, string interactionPoint)
    {
      RequestData request = new TargetedMessagesRequestData(_shopperContext.Value.ShopperId, _siteContext.Value.PrivateLabelId.ToString(), appId, interactionPoint);
      TargetedMessagesResponseData response = SessionCache.SessionCache.GetProcessRequest<TargetedMessagesResponseData>(request, PersonalizationEngineRequests.RequestId);

      return response.TargetedMessagesData;
    }
  }
}
