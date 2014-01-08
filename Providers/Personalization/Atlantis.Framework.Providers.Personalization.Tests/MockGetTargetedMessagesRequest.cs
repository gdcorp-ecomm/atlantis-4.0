﻿using Atlantis.Framework.Interface;
using Atlantis.Framework.Personalization.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.Personalization.Tests
{
  public class MockGetTargetedMessagesRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      TargetedMessagesRequestData requestData = (TargetedMessagesRequestData)oRequestData;
      TargetedMessagesResponseData responseData = new TargetedMessagesResponseData(GetTargetedMessages(requestData.InteractionPoint), requestData.InteractionPoint);
      return responseData;
    }

    private string GetTargetedMessages(string interactionPoint)
    {

      if (interactionPoint == "explode")
      {
        throw new Exception("exploding");
      }

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

      return testXml;
    }
  }
}
