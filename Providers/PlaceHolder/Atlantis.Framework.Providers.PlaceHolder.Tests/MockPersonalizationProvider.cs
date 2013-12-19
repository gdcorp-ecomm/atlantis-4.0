using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;


namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  public class MockPersonalizationProvider : ProviderBase, IPersonalizationProvider
  {
    public MockPersonalizationProvider(IProviderContainer container)
      : base(container)
    {
    }

    public TargetedMessages GetTargetedMessages(string interactionPoint)
    {

      if (interactionPoint == "explode")
      {
        throw new Exception("exploding");
      }

      TargetedMessages messages = null;
      string testXml =
        @"<TargetedMessages>
  <Messages>

    <Message>
      <MessageID>Id1</MessageID>
      <MessageName>engmtactnewcustsurvey</MessageName>
      <MessageSequence>0</MessageSequence>
      <MessageTags>
        <MessageTag>
          <Name>engmtactnewcustsurveywebdlp</Name>
        </MessageTag>
        <MessageTag>
          <Name>engmtactnewcustsurveymobiledlp</Name>
        </MessageTag>
      </MessageTags>
      <MessageTrackingID>1</MessageTrackingID>
    </Message>

    <Message>
      <MessageID>Id2</MessageID>
      <MessageName>stddomxsdom</MessageName>
      <MessageSequence>1</MessageSequence>
      <MessageTags>
        <MessageTag>
          <Name>stddomxswebhp</Name>
        </MessageTag>
        <MessageTag>
          <Name>stddomxswebdlp</Name>
        </MessageTag>
        <MessageTag>
          <Name>stddomxsmobilehp</Name>
        </MessageTag>
        <MessageTag>
          <Name>stddomxsmobiledlp</Name>
        </MessageTag>
      </MessageTags>
      <MessageTrackingID>2</MessageTrackingID>
    </Message>

    <Message>
      <MessageID>Id3</MessageID>
      <MessageName>duplicate1</MessageName>
      <MessageSequence>2</MessageSequence>
      <MessageTags>
        <MessageTag>
          <Name>duplicate</Name>
        </MessageTag>
      </MessageTags>
      <MessageTrackingID>3</MessageTrackingID>
    </Message>

    <Message>
      <MessageID>Id4</MessageID>
      <MessageName>duplicate2</MessageName>
      <MessageSequence>2</MessageSequence>
      <MessageTags>
        <MessageTag>
          <Name>duplicate</Name>
        </MessageTag>
      </MessageTags>
      <MessageTrackingID>4</MessageTrackingID>
    </Message>

    <Message>
      <MessageID>Id5</MessageID>
      <MessageName>duplicate3</MessageName>
      <MessageSequence>2</MessageSequence>
      <MessageTags>
        <MessageTag>
          <Name>duplicate</Name>
        </MessageTag>
      </MessageTags>
      <MessageTrackingID>5</MessageTrackingID>
    </Message>

  </Messages>
</TargetedMessages>";

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
      catch (Exception ex)
      {
        ErrorLogger.LogException(ex.Message, "MockPersonalizationProvider", string.Empty);
      }

      return messages;
    }
  }
}
