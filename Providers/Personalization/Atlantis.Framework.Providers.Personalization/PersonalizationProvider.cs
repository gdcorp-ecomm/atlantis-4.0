using Atlantis.Framework.Interface;
using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;
using System;
using System.Collections.Generic;
using Atlantis.Framework.Engine;

namespace Atlantis.Framework.Providers.Personalization
{
  public class PersonalizationProvider : ProviderBase, IPersonalizationProvider
  {
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<IShopperContext> _shopperContext;

    private Lazy<ShopperSpecificSessionDataItem<TargetedMessages>> _targetedMessagesSessionData =
               new Lazy<ShopperSpecificSessionDataItem<TargetedMessages>>(() => { return new ShopperSpecificSessionDataItem<TargetedMessages>("PersonalizationProvider.TargetedMessages"); });


    public PersonalizationProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
    }

    public TargetedMessages GetTargetedMessages(string interactionPoint)
    {
      TargetedMessages targetedMessages;

      if (!_targetedMessagesSessionData.Value.TryGetData(_shopperContext.Value.ShopperId, out targetedMessages))
      {
        RequestData request = new TargetedMessagesRequestData(_shopperContext.Value.ShopperId, _siteContext.Value.PrivateLabelId.ToString(), PersonalizationConfig.TMSAppId, interactionPoint);
        targetedMessages = ((TargetedMessagesResponseData)Engine.Engine.ProcessRequest(request, PersonalizationEngineRequests.RequestId)).TargetedMessagesData;
        _targetedMessagesSessionData.Value.SetData(_shopperContext.Value.ShopperId, targetedMessages);
      }
            
      return targetedMessages;
    }
  }
}
