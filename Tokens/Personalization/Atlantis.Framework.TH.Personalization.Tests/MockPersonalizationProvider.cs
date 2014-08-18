using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;

namespace Atlantis.Framework.TH.Personalization.Tests
{
  public class MockPersonalizationProvider : ProviderBase, IPersonalizationProvider
  {
    public MockPersonalizationProvider(IProviderContainer container)
      : base(container)
    {
    }

    public void AddConsumedMessage(IConsumedMessage message)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<IConsumedMessage> GetConsumedMessages()
    {
      throw new NotImplementedException();
    }

    public TargetedMessages GetTargetedMessages(string interactionPoint)
    {
      TargetedMessages messages = null;
      string testXml =
        @"<TargetedMessages xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance\"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema\""><SessionID>25A28019-3D57-45D4-BC21-4C9DFB51421A</SessionID><ResultCode>0</ResultCode><Messages>
                        <Message><MessageTrackingID>aG9tZVBhZ2V8TWFya2V0aW5nLmJlc3RCZW5lZml0fE1hcmtldGluZy5lbmdhZ2VDdXN0b21lcnN8TWFya2V0aW5nLmN1c3RvbWVyU2F0fENoYW5uZWwud2ViLnBlcnNvbmFsaXplZEhvbWV8d2Vi</MessageTrackingID><MessageID>7f8426c1-e9de-491f-9bcb-19fd8351d22a</MessageID>
                        <MessageName>EngmtActNewCustSurvey</MessageName><MessageSequence>0</MessageSequence>
                        <MessageTags><MessageTag><Name>EngmtActNewCustSurveyMobileDLP</Name></MessageTag><MessageTag><Name>EngmtActNewCustSurveyWebDLP</Name></MessageTag></MessageTags></Message>
                        <Message><MessageTrackingID>aG9tZVBhZ2V8TWFya2V0aW5nLmJlc3RCZW5lZml0fE1hcmtldGluZy5jdXN0b21lclNhdGlzZmFjdGlvbnxNYXJrZXRpbmcuY3VzdG9tZXJTYXR8Q2hhbm5lbC53ZWIucGVyc29uYWxpemVkSG9tZXx3ZWI=</MessageTrackingID><MessageID>0e554af3-9851-4a99-90f0-1d11987ea553</MessageID>
                        <MessageName>EngmtCustServMobileApp</MessageName><MessageSequence>1</MessageSequence>
                        <MessageTags><MessageTag><Name>EngmtCustServMobileAppMobileHP</Name></MessageTag><MessageTag><Name>EngmtCustServMobileAppWebHP</Name></MessageTag></MessageTags></Message>
                        </Messages></TargetedMessages>";

      try
      {
        var targetMessageXml = XDocument.Parse(testXml);

        if (targetMessageXml.Root != null)
        {
          using (XmlReader reader = targetMessageXml.Root.CreateReader())
          {
            XmlSerializer serializer = new XmlSerializer(typeof(TargetedMessages), targetMessageXml.Root.GetDefaultNamespace().NamespaceName);
            messages = (TargetedMessages)serializer.Deserialize(reader);
            reader.Close();
          }
        }
      }
      catch (Exception)
      {
        Debug.WriteLine("GetTargetMessages Deserialization Failed.");
      }

      return messages;
    }

    public string TrackingData { get; private set; }
    public IEnumerable<IConsumedMessage> ConsumedMessages { get; private set; }
  }
}
