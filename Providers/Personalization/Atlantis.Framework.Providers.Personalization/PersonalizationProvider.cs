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
    private readonly Lazy<IDebugContext> _debugContext;

    private Lazy<ShopperSpecificSessionDataItem<TargetedMessagesResponseData>> _targetedMessagesSessionData =
               new Lazy<ShopperSpecificSessionDataItem<TargetedMessagesResponseData>>(() => { return new ShopperSpecificSessionDataItem<TargetedMessagesResponseData>("PersonalizationProvider.TargetedMessages"); });


    public PersonalizationProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
      _debugContext = new Lazy<IDebugContext>(() => Container.Resolve<IDebugContext>());
    }

    public TargetedMessages GetTargetedMessages(string interactionPoint)
    {
      TargetedMessages messages = null;
      TargetedMessagesResponseData response;

      if (string.IsNullOrEmpty(PersonalizationConfig.TMSAppId))
      {
        throw new ApplicationException("Config value, \"PersonalizationConfig.TMSAppId\" is empty.  Pleas set this in your application start event.");
      }

      if (!_targetedMessagesSessionData.Value.TryGetData(_shopperContext.Value.ShopperId, out response))
      {
        RequestData request = new TargetedMessagesRequestData(_shopperContext.Value.ShopperId, _siteContext.Value.PrivateLabelId.ToString(), PersonalizationConfig.TMSAppId, interactionPoint);
        response = (TargetedMessagesResponseData)Engine.Engine.ProcessRequest(request, PersonalizationEngineRequests.RequestId);
        _targetedMessagesSessionData.Value.SetData(_shopperContext.Value.ShopperId, response);
        if (response != null)
        {
          if (_siteContext.Value.IsRequestInternal)
          {
            _debugContext.Value.LogDebugTrackingData("TMS Service URL", response.TMSUrl);
          }
        }
      }

      if (response != null)
      {
        messages = response.TargetedMessagesData;
      }
      
      return messages;
    }
  }
}
