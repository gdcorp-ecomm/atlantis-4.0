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

      bool foundInSession = _targetedMessagesSessionData.Value.TryGetData(_shopperContext.Value.ShopperId, interactionPoint, _siteContext.Value.PrivateLabelId.ToString(), out response);

      if (foundInSession)
      {
        messages = response.TargetedMessagesData;
        if (_siteContext.Value.IsRequestInternal)
        {
          _debugContext.Value.LogDebugTrackingData("TMS Service URL (from Session)", response.TMSUrl);
        }
      }
      else 
      {
        RequestData request = new TargetedMessagesRequestData(_shopperContext.Value.ShopperId, _siteContext.Value.PrivateLabelId.ToString(), PersonalizationConfig.TMSAppId, interactionPoint);
        response = (TargetedMessagesResponseData)Engine.Engine.ProcessRequest(request, PersonalizationEngineRequests.RequestId);
        if (response != null)
        {
          _targetedMessagesSessionData.Value.SetData(_shopperContext.Value.ShopperId, interactionPoint, _siteContext.Value.PrivateLabelId.ToString(), response);
          messages = response.TargetedMessagesData;
          if (_siteContext.Value.IsRequestInternal)
          {
            _debugContext.Value.LogDebugTrackingData("TMS Service URL", response.TMSUrl);
          }
        }
      }
      
      return messages;
    }
  }
}
