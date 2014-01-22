using Atlantis.Framework.Interface;
using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;
using System;
using System.Collections.Generic;
using Atlantis.Framework.Engine;
using System.Text;
using System.Web;
using System.Linq;
using Atlantis.Framework.Providers.AppSettings.Interface;

namespace Atlantis.Framework.Providers.Personalization
{
  public class PersonalizationProvider : ProviderBase, IPersonalizationProvider
  {
    const string DELIM = "^";
    const string CONTAINER_KEY = "A.F.PersonalizationProvider.ConsumedMessages";
    const string DATA_TOKEN_MESSAGE_Id = "TMSMessageId";
    const string DATA_TOKEN_MESSAGE_NAME = "TMSMessageName";
    const string DATA_TOKEN_MESSAGE_TAG = "TMSMessageTag";
    const string DATA_TOKEN_MESSAGE_TRACKING_ID = "TMSMessageTrackingId";
    const string AppSetting = "A.F.Prov.Personalization.TMS.On";
    const int TIMEOUT_MILLISECONDS = 100;

    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<IShopperContext> _shopperContext;
    private readonly Lazy<IDebugContext> _debugContext;
    private readonly Lazy<IAppSettingsProvider> _appSettingsProvider;
    private readonly List<IConsumedMessage> _emptyMessages;
    private readonly Lazy<string> _trackingData;
    
    private Lazy<ShopperSpecificSessionDataItem<TargetedMessagesResponseData>> _targetedMessagesSessionData =
               new Lazy<ShopperSpecificSessionDataItem<TargetedMessagesResponseData>>(() => { return new ShopperSpecificSessionDataItem<TargetedMessagesResponseData>("PersonalizationProvider.TargetedMessages"); });


    public PersonalizationProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
      _debugContext = new Lazy<IDebugContext>(() => Container.Resolve<IDebugContext>());
      _trackingData = new Lazy<string>(() => GetTrackingData());
      _appSettingsProvider = new Lazy<IAppSettingsProvider>(() => Container.Resolve<IAppSettingsProvider>());
      _emptyMessages = new List<IConsumedMessage>();
    }

    private const string COOKIE_VISITOR = "visitor";
    private const string COOKIE_VALUE_KEY = "vid";
    internal string VisitorGuid
    {
      get
      {
        string visitorGuid = string.Empty;
        HttpCookie visitorCookie = HttpContext.Current.Request.Cookies[COOKIE_VISITOR];
        if (visitorCookie != null && visitorCookie[COOKIE_VALUE_KEY] != null)
        {
          visitorGuid = visitorCookie[COOKIE_VALUE_KEY];
        }
        return visitorGuid;
      }
    }

    public virtual TargetedMessages GetTargetedMessages(string interactionPoint)
    {
      TargetedMessages messages = null;

      TargetedMessagesRequestData request = new TargetedMessagesRequestData(_shopperContext.Value.ShopperId, _siteContext.Value.PrivateLabelId.ToString(), PersonalizationConfig.TMSAppId, interactionPoint, VisitorGuid, _siteContext.Value.ISC);
      request.RequestTimeout = TimeSpan.FromMilliseconds(TIMEOUT_MILLISECONDS);
      try
      {
        bool isTMSOn = _appSettingsProvider.Value.GetAppSetting(AppSetting).Equals("true", StringComparison.OrdinalIgnoreCase);

        if (isTMSOn)
        {
          TargetedMessagesResponseData response;

          if (string.IsNullOrEmpty(PersonalizationConfig.TMSAppId))
          {
            throw new ApplicationException("Config value, \"PersonalizationConfig.TMSAppId\" is empty.  Pleas set this in your application start event.");
          }

          bool foundInSession = _targetedMessagesSessionData.Value.TryGetData(request, out response);

          if (foundInSession)
          {
            messages = response.TargetedMessagesData;
            LogDebugMessage("TMS Service URL (from Session)", response.TMSUrl);
          }
          else
          {
            response = (TargetedMessagesResponseData)Engine.Engine.ProcessRequest(request, PersonalizationEngineRequests.RequestId);
            if (response != null)
            {
              _targetedMessagesSessionData.Value.SetData(request, response);
              messages = response.TargetedMessagesData;
              LogDebugMessage("TMS Service URL", response.TMSUrl);
            }
          }
        }
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException(request,
                                                    "PersonalizationProvider.GetTargetedMessages",
                                                    ex.ToString(),
                                                    request.GetWebServicePath(), 
                                                    ex);

        Engine.Engine.LogAtlantisException(aex);
      }
      
      return messages;
    }

    private void LogDebugMessage(string key, string value)
    {
      if (_siteContext.Value.IsRequestInternal)
      {
        _debugContext.Value.LogDebugTrackingData(key, value);
      }
    }
    
    public void AddToConsumedMessages(IConsumedMessage message)
    {
      Container.SetData<string>(DATA_TOKEN_MESSAGE_Id, message.MessageId);
      Container.SetData<string>(DATA_TOKEN_MESSAGE_TAG, message.TagName);
      Container.SetData<string>(DATA_TOKEN_MESSAGE_NAME, message.MessageName);
      Container.SetData<string>(DATA_TOKEN_MESSAGE_TRACKING_ID, message.TrackingId);

      var messages = Container.GetData<List<IConsumedMessage>>(CONTAINER_KEY, _emptyMessages);
      messages.Add(message);
      Container.SetData<List<IConsumedMessage>>(CONTAINER_KEY, messages);
    }

    private string GetTrackingData()
    {
        string trackingData = string.Empty;

        var messages = Container.GetData<List<IConsumedMessage>>(CONTAINER_KEY, null);

        if (messages != null)
        {
          StringBuilder sb = new StringBuilder(messages.Count * 50);
          foreach (IConsumedMessage message in messages)
          {
            if (sb.Length > 0) sb.Append(DELIM);
            sb.Append(message.TrackingData);
          }
          trackingData = sb.ToString();
        }

        return trackingData;
    }

    public IEnumerable<IConsumedMessage> ConsumedMessages
    {
      get
      {
        return Container.GetData<List<IConsumedMessage>>(CONTAINER_KEY, _emptyMessages) as IEnumerable<IConsumedMessage>;
      }
    }
    
    public string TrackingData
    {
      get { return _trackingData.Value; }
    }
  }
}
