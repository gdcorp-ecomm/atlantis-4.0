using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.TMSContent;
using Atlantis.Framework.Providers.TMSContent.Interface;
using Newtonsoft.Json.Linq;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  [ExcludeFromCodeCoverage]
  public class MockTMSContentProvider : ProviderBase, ITMSContentProvider
  {
    public string TrackingData { get; private set; }

    public MockTMSContentProvider(IProviderContainer container)
      : base(container) {}

    #region ITMSContentProvider Members

    public void ConsumeMessage(string appProduct, string interactionPoint, string channel, IMessageVariant message) { }

    public string GetTrackingData()
    {
      return null;
    }

    public bool TryGetMessages(string shopper_id, string appProduct, string interactionName, string channel,
      JObject postData, out IList<IMessageVariant> messageVariants, Func<IMessageVariant, bool> predicate)
    {
      messageVariants = null;

      if (interactionName == "FailGetMessages")
      {
        return false;
      }

      if (interactionName == "NoContent")
      {
        JObject jObject = JObject.Parse(@"{
          'tag': 'NoContentTag',
          'name': 'NoContentName',
          'strategy': 'NoContentStrategy',
          'message_id': 'ID1',
          'tracking_id': 'ID1',
          'data': {
            'key1': 'value1',
            'key2': 'value2'
          }
        }");
        messageVariants = new List<IMessageVariant>
        {
          new MessageVariant(TMSContentConfig.AppID, appProduct, interactionName, channel, jObject)
        };
        return true;
      }

      // First Response
      if (interactionName.Equals("First", StringComparison.OrdinalIgnoreCase))
      {
        JObject jObject = JObject.Parse(@"{
          'tag': 'Tag1',
          'name': 'message1',
          'strategy': 'Strategy1',
          'message_id': 'ID1',
          'tracking_id': 'ID1',
          'data': {
            'key1': 'value1',
            'key2': 'value2'
          }
        }");
        messageVariants = new List<IMessageVariant>
        {
          new MessageVariant(TMSContentConfig.AppID, appProduct, interactionName, channel, jObject)
        };
        return true;
      }

      // Second Response
      if (interactionName.Equals("Second", StringComparison.OrdinalIgnoreCase))
      {
        JObject jObject = JObject.Parse(@"{
          'tag': 'Tag2',
          'name': 'Message2',
          'strategy': 'Strategy2',
          'message_id': 'ID2',
          'tracking_id': 'ID2',
          'data': {
            'key1': 'value1',
            'key2': 'value2'
          }
        }");
        messageVariants = new List<IMessageVariant>
        {
          new MessageVariant(TMSContentConfig.AppID, appProduct, interactionName, channel, jObject)
        };
        return true;
      }

      // Multiple Response
      if (interactionName.Equals("Multiple", StringComparison.OrdinalIgnoreCase))
      {
        JObject jObject1 = JObject.Parse(@"{
          'tag': 'Tag1',
          'name': 'Message1',
          'strategy': 'Strategy1',
          'message_id': 'ID1',
          'tracking_id': 'ID1',
          'hasContent': true,
          'data': {
            'key1': 'value1',
            'key2': 'value2'
          }
        }");

        JObject jObject2 = JObject.Parse(@"{
           'tag': 'Tag2',
           'name': 'Message2',
           'strategy': 'Strategy2',
           'message_id': 'ID2',
           'tracking_id': 'ID2',
           'data': {
             'key1': 'value1',
             'key2': 'value2'
           }
        }");
        messageVariants = new List<IMessageVariant>
        {
          new MessageVariant(TMSContentConfig.AppID, appProduct, interactionName, channel, jObject1),
          new MessageVariant(TMSContentConfig.AppID, appProduct, interactionName, channel, jObject2)
        };
        return true;
      }

      return false;
    }

    #endregion
  }
}
