using System;
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
    private bool _success = false;

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public TargetedMessages TargetedMessagesData { get; private set; }

    public TargetedMessagesResponseData(string xml)
    {
      try
      {
        _resultXML = xml;
        _success = true;
        TargetedMessagesData = BuildTargetedMessages();
      }
      catch (Exception ex)
      {
        this._exception = new AtlantisException(null, "TargetedMessagesResponseData", ex.Message, TargetedMessagesXml.ToString());
        _success = false;
      }
    }

    public TargetedMessagesResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
      _success = false;
    }

    public TargetedMessagesResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData, "TargetedMessagesResponseData", exception.Message, requestData.ToXML());
      _success = false;
    }

    private TargetedMessages BuildTargetedMessages()
    {
      TargetedMessages messages = new TargetedMessages();
      XmlReader reader = TargetedMessagesXml.Root.CreateReader();
      XmlSerializer serializer = new XmlSerializer(typeof(TargetedMessages), TargetedMessagesXml.Root.GetDefaultNamespace().NamespaceName);
      messages = (TargetedMessages)serializer.Deserialize(reader);
      reader.Close();
      return messages;
    }

    private XDocument _targetedMessagesXml;
    private XDocument TargetedMessagesXml
    {
      get
      {
        if (_targetedMessagesXml == null)
        {
          string parseError = null;
          if (!string.IsNullOrEmpty(_resultXML))
          {
            try
            {
              _targetedMessagesXml = XDocument.Parse(_resultXML);
            }
            catch (Exception ex)
            {
              parseError = ex.Message;
              _targetedMessagesXml = null;
            }
          }

          if (_targetedMessagesXml == null)
          {
            XElement root = new XElement("TargetedMessages");
            if (parseError != null)
            {
              XElement parseErrorElement = new XElement("ParseError", parseError);
              root.Add(parseErrorElement);
            }
            _targetedMessagesXml = new XDocument(root);
          }
        }
        return _targetedMessagesXml;
      }
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
      if (!string.IsNullOrEmpty(sessionData))
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(TargetedMessagesResponseData));
        StringReader reader = new StringReader(sessionData);
        TargetedMessagesResponseData responseData = xmlSerializer.Deserialize(reader) as TargetedMessagesResponseData;
        if (!ReferenceEquals(null, responseData))
        {
          TargetedMessagesData = responseData.TargetedMessagesData;
        }
      }
    }
    #endregion

  }
}
