using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.PlaceHolders;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Providers.TMSContent.Interface;
using Atlantis.Framework.Providers.TMSContentData.Interface;
using Atlantis.Framework.Providers.Web.Interface;
using Atlantis.Framework.Render.Containers;
using Newtonsoft.Json.Linq;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  internal class TMSContentPlaceHolderHandler : CDSDocumentPlaceHolderHandler
  {
    private const string CONTEXT_DATA_PREFIX = "tms.message";

    private const string TRACKING_DIV_FORMAT_NAME = "data-tms-name='{0}'";
    private const string TRACKING_DIV_FORMAT_RANK = "data-tms-rank='{0}'";
    private const string TRACKING_DIV_FORMAT_CHANNEL = "data-tms-channel='{0}'";
    private const string TRACKING_DIV_FORMAT_STRATEGY = "data-tms-strategy='{0}'";
    private const string TRACKING_DIV_FORMAT_TEMPLATE = "data-tms-template='{0}'";
    private const string TRACKING_DIV_FORMAT_TRACKING_ID = "data-tms-trackingid='{0}'";

    private const char TMS_SPOOF_DELIM = ';';
    private const char TMS_SPOOF_KEY_DELIM = '|';
    private const string TMS_SPOOF_KEY = "tmsPlaceholderSpoof";

    private readonly Lazy<ICDSContentProvider> _cdsContentProvider;
    private readonly Lazy<IDebugContext> _debugContextProvider;
    private readonly Lazy<IRenderPipelineProvider> _renderPipelineProvider;
    private readonly Lazy<ISiteContext> _siteContextProvider;
    private readonly Lazy<ITMSContentDataProvider> _tmsContentDataProvider;
    private readonly Lazy<ITMSContentProvider> _tmsContentProvider;
    private readonly Lazy<IWebContext> _webContextProvider;

    internal TMSContentPlaceHolderHandler(IPlaceHolderHandlerContext context)
      : base(context)
    {
      _cdsContentProvider = new Lazy<ICDSContentProvider>(() => context.ProviderContainer.Resolve<ICDSContentProvider>());
      _renderPipelineProvider = new Lazy<IRenderPipelineProvider>(() => context.ProviderContainer.Resolve<IRenderPipelineProvider>());
      _tmsContentProvider = new Lazy<ITMSContentProvider>(() => context.ProviderContainer.Resolve<ITMSContentProvider>());
      _tmsContentDataProvider = new Lazy<ITMSContentDataProvider>(() => context.ProviderContainer.Resolve<ITMSContentDataProvider>());

      // Optional Provider(s)
      _debugContextProvider = new Lazy<IDebugContext>(() =>
      {
        IDebugContext value;
        return context.ProviderContainer.TryResolve(out value) ? value : null;
      });

      _siteContextProvider = new Lazy<ISiteContext>(() =>
      {
        ISiteContext value;
        return context.ProviderContainer.TryResolve(out value) ? value : null;
      });

      _webContextProvider = new Lazy<IWebContext>(() =>
      {
        IWebContext value;
        return context.ProviderContainer.TryResolve(out value) ? value : null;
      });
    }

    protected override string GetContent()
    {
      try
      {
        TMSContentPlaceHolderData placeHolderData = new TMSContentPlaceHolderData(Context.Data);
        string content;

        // Spoof Content
        if (TryGetSpoofContent(placeHolderData, out content))
        {
          return content;
        }
        if (placeHolderData.Rank.HasValue && TryGetRankedContent(placeHolderData, out content))
        {
          return content;
        }
        if (!placeHolderData.Rank.HasValue && TryGetConsumableContent(placeHolderData, out content))
        {
          return content;
        }
        if (TryGetDefaultContent(placeHolderData, out content))
        {
          return content;
        }

        throw new SystemException(
          string.Format("Failed to load any content for placeholder. '{0}'", placeHolderData.Serialize()));
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("PlaceHolder render error. Type: {0}, Message: {1}", Context.Type, ex.Message);
        LogError(errorMessage, "TMSContentPlaceHolderHandler.GetContent()");
        return string.Empty;
      }
    }

    private bool TryGetDefaultContent(TMSContentPlaceHolderData placeHolderData, out string content)
    {
      if (_tmsContentProvider.Value.TryGetDefaultContent(placeHolderData.Template, placeHolderData.Channel, out content))
      {
        SetTrackingData(placeHolderData, null, ref content);
        return true;
      }
      return false;
    }

    private bool TryGetConsumableContent(TMSContentPlaceHolderData placeHolderData, out string content)
    {
      string shopper_id = _tmsContentDataProvider.Value.GetShopperID();
      JObject postData = _tmsContentDataProvider.Value.GetPostData();
      content = null;

      // Consumption-Based Selection
      IList<IMessageVariant> messages;
      if (_tmsContentProvider.Value.TryGetMessages(shopper_id, placeHolderData.Product, placeHolderData.Interaction,
        placeHolderData.Channel, postData, out messages, message => !message.IsConsumed))
      {
        IMessageVariant message = messages.FirstOrDefault();
        if ((message != null) && (message.IsControl ?
          _tmsContentProvider.Value.TryGetDefaultContent(placeHolderData.Template, placeHolderData.Channel, out content) :
          _tmsContentProvider.Value.TryGetMessageContent(placeHolderData.Template, message, out content)))
        {
          _tmsContentProvider.Value.ConsumeMessage(placeHolderData.Product, placeHolderData.Interaction,
            placeHolderData.Channel, message);
          SetContextData(message, CONTEXT_DATA_PREFIX);
          SetTrackingData(placeHolderData, message, ref content);
          content = _renderPipelineProvider.Value.RenderContent(content,
            new IRenderHandler[] { new ProviderContainerDataTokenRenderHandler() });
          return true;
        }
      }
      return false;
    }

    private bool TryGetRankedContent(TMSContentPlaceHolderData placeHolderData, out string content)
    {
      string shopper_id = _tmsContentDataProvider.Value.GetShopperID();
      JObject postData = _tmsContentDataProvider.Value.GetPostData();
      content = null;

      // Rank-Based Selection
      IList<IMessageVariant> messages;
      if (_tmsContentProvider.Value.TryGetMessages(shopper_id, placeHolderData.Product, placeHolderData.Interaction,
        placeHolderData.Channel, postData, out messages) && placeHolderData.Rank.HasValue)
      {
        IMessageVariant message = messages.ElementAtOrDefault(placeHolderData.Rank.Value);
        if ((message != null) && (message.IsControl ?
          _tmsContentProvider.Value.TryGetDefaultContent(placeHolderData.Template, placeHolderData.Channel, out content) :
          _tmsContentProvider.Value.TryGetMessageContent(placeHolderData.Template, message, out content)))
        {
          _tmsContentProvider.Value.ConsumeMessage(placeHolderData.Product, placeHolderData.Interaction,
            placeHolderData.Channel, message);
          SetContextData(message, CONTEXT_DATA_PREFIX);
          SetTrackingData(placeHolderData, message, ref content);
          content = _renderPipelineProvider.Value.RenderContent(content,
            new IRenderHandler[] { new ProviderContainerDataTokenRenderHandler() });
          return true;
        }
      }
      return false;
    }

    private void SetContextData(IMessageVariant message, string prefix)
    {
      SetContextData(message.ToJson(), prefix);
    }

    private void SetContextData(JObject jObject, string prefix)
    {
      foreach (JProperty property in jObject.Properties())
      {
        string contextKey = prefix + "." + property.Name;
        switch (property.Value.Type)
        {
          case JTokenType.Boolean:
          case JTokenType.Date:
          case JTokenType.Float:
          case JTokenType.Integer:
          case JTokenType.String:
            Context.ProviderContainer.SetData(contextKey.ToLower(), property.Value.ToObject<object>());
            break;
          case JTokenType.Object:
            SetContextData(property.Value.ToObject<JObject>(), contextKey);
            break;
        }
      }
    }

    private void SetTrackingData(TMSContentPlaceHolderData placeHolderData, IMessageVariant message, ref string content)
    {
      content = string.Join(" ", "<div",
        (message != null) ? string.Format(TRACKING_DIV_FORMAT_NAME, message.IsControl ? message.Control_Name : message.Name) : string.Empty,
        (message != null) ? string.Format(TRACKING_DIV_FORMAT_STRATEGY, message.Strategy) : string.Empty,
        (message != null) ? string.Format(TRACKING_DIV_FORMAT_TRACKING_ID, message.TrackingID) : string.Empty,
        string.Format(TRACKING_DIV_FORMAT_CHANNEL, placeHolderData.Channel),
        string.Format(TRACKING_DIV_FORMAT_TEMPLATE, placeHolderData.Template),
        string.Format(TRACKING_DIV_FORMAT_RANK, placeHolderData.Rank), ">", content ?? string.Empty, "</div>");
    }

    [ExcludeFromCodeCoverage]
    private bool TryGetSpoofContent(TMSContentPlaceHolderData placeHolderData, out string content)
    {
      content = null;

      if ((_webContextProvider.Value != null) && _webContextProvider.Value.HasContext &&
          (_webContextProvider.Value.ContextBase != null) &&
          (_webContextProvider.Value.ContextBase.Request != null) &&
          (_webContextProvider.Value.ContextBase.Request.QueryString != null) &&
          (_siteContextProvider.Value != null) && _siteContextProvider.Value.IsRequestInternal)
      {
        string[] spoofValuesList = _webContextProvider.Value.ContextBase.Request.QueryString.GetValues(TMS_SPOOF_KEY);
        if (spoofValuesList != null)
        {
          foreach (string spoofValues in spoofValuesList)
          {
            // Format: spoofPlaceholderContent=app|tmsv2;location|content/homepage/499com;product|hp;interaction|marquee;channel|homepage;template|hero;rank|0
            int? rank = null;
            string app = null, location = null, product = null, interaction = null, channel = null, template = null;
            foreach (string value in spoofValues.Split(new[] {TMS_SPOOF_DELIM}, StringSplitOptions.RemoveEmptyEntries).Select(HttpUtility.UrlDecode))
            {
              string[] valueSplits = value.Split(new []{TMS_SPOOF_KEY_DELIM}, StringSplitOptions.RemoveEmptyEntries);
              if (valueSplits.Length == 2)
              {
                string spoofKey = valueSplits[0].ToLower();
                string spoofValue = valueSplits[1].ToLower();
                switch (spoofKey)
                {
                  case "app":
                    app = spoofValue;
                    break;
                  case "location":
                    location = spoofValue;
                    break;
                  case "product":
                    product = spoofValue;
                    break;
                  case "interaction":
                    interaction = spoofValue;
                    break;
                  case "channel":
                    channel = spoofValue;
                    break;
                  case "template":
                    template = spoofValue;
                    break;
                  case "rank":
                    int nValue;
                    rank = (int.TryParse(spoofValue, out nValue)) ? (int?) nValue : null;
                    break;
                }
              }
            }

            if (!string.IsNullOrEmpty(app) && !string.IsNullOrEmpty(location) &&
                string.Equals(placeHolderData.Product, product, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(placeHolderData.Interaction, interaction, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(placeHolderData.Channel, channel, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(placeHolderData.Template, template, StringComparison.OrdinalIgnoreCase) &&
                (!placeHolderData.Rank.HasValue || (placeHolderData.Rank == rank)))
            {
              // Spoof Path: {Location}/{Template||Interaction}
              StringBuilder path = new StringBuilder(256);

              // Set {Location}
              path.Append(location.Replace('\\', '/') + "/");

              // Set {Template}
              path.Append(template);

              // Get Content
              content = _cdsContentProvider.Value.GetContent(app, path.ToString()).Content;
              if (string.IsNullOrEmpty(content))
              {
                TryLogDebugMessage(
                  string.Format("TMS Spoof ({0}/{1}){{{2}}}", product, interaction, channel),
                  string.Format("{0}; {1}/{2}", (int)HttpStatusCode.NotFound, app, path));
                return false;
              }

              TryLogDebugMessage(
                string.Format("TMS Spoof ({0}/{1}){{{2}}}", product, interaction, channel),
                string.Format("{0}; {1}/{2}", (int)HttpStatusCode.OK, app, path));
              SetTrackingData(placeHolderData, null, ref content);
              return true;
            }
          }
        }
      }
      return false;
    }

    [ExcludeFromCodeCoverage]
    private void TryLogDebugMessage(string key, string value)
    {
      if ((_siteContextProvider.Value != null) && _siteContextProvider.Value.IsRequestInternal &&
          (_debugContextProvider.Value != null))
      {
        _debugContextProvider.Value.LogDebugTrackingData(key, value);
      }
    }
  }
}
