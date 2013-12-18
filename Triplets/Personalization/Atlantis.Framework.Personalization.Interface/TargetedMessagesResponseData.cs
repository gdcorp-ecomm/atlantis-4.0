using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Personalization.Interface
{
    [Serializable]
    public class TargetedMessagesResponseData : IResponseData
  {
    public TargetedMessagesResponseData() { }

    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;

    public TargetedMessages TargetedMessagesData { get; private set; }
    public string TMSUrl { get; private set; }

    public TargetedMessagesResponseData(string xml, string tmsURL)
    {
      try
      {
        _resultXML = xml;
        TMSUrl = tmsURL;
        TargetedMessagesData = BuildTargetedMessages(_resultXML);
      }
      catch (Exception ex)
      {
        _exception = new AtlantisException(null, "TargetedMessagesResponseData", ex.Message, _resultXML);
      }
    }

    public TargetedMessagesResponseData(RequestData requestData, Exception exception, string tmsURL)
    {
      TMSUrl = tmsURL;
      _exception = new AtlantisException(requestData, "TargetedMessagesResponseData", exception.Message, requestData.ToXML());
    }

    private TargetedMessages BuildTargetedMessages(string xmlResult)
    {
      TargetedMessages messages = new TargetedMessages();

      try
      {
        var targetMessageXml = XDocument.Parse(xmlResult);

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

      TargetedMessagesData = BuildTargetedMessages(sessionData);
    }
    #endregion

  }
}
