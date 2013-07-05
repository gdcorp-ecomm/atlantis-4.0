﻿using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.Personalization.Interface
{
  public class TargetedMessagesResponseData : IResponseData, ISessionSerializableResponse
  {
    public TargetedMessagesResponseData(){}

    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;

    public TargetedMessages TargetedMessagesData { get; private set; }

    public TargetedMessagesResponseData(string xml)
    {
      try
      {
        _resultXML = xml;
        TargetedMessagesData = BuildTargetedMessages();
      }
      catch (Exception ex)
      {
        _exception = new AtlantisException(null, "TargetedMessagesResponseData", ex.Message, _resultXML);
      }
    }

    public TargetedMessagesResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData, "TargetedMessagesResponseData", exception.Message, requestData.ToXML());
    }

    private TargetedMessages BuildTargetedMessages()
    {
      TargetedMessages messages = new TargetedMessages();

      try
      {
        var targetMessageXml = XDocument.Parse(_resultXML);

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
        _exception = new AtlantisException(null, "TargetedMessagesResponseData.BuildTargetedMessages", ex.Message, _resultXML);
      }

      return messages;
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

    #region ISessionSerializableResponse Members
    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      if (string.IsNullOrEmpty(sessionData)) return;

      XmlSerializer xmlSerializer = new XmlSerializer(typeof(TargetedMessagesResponseData));
      StringReader reader = new StringReader(sessionData);
      TargetedMessagesResponseData responseData = xmlSerializer.Deserialize(reader) as TargetedMessagesResponseData;
      if (responseData != null)
      {
        TargetedMessagesData = responseData.TargetedMessagesData;
      }
    }
    #endregion

  }
}