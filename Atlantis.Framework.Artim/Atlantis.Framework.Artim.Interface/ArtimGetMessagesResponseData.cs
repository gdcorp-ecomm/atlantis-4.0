using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Artim.Interface
{
  public class ArtimGetMessagesResponseData : IResponseData
  {

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

    public ArtimGetMessagesResponseData(string xml)
    {
      try
      {
        _resultXML = xml;
        _success = true;
        TargetedMessagesData = BuildTargetedMessages();
      }
      catch (Exception ex)
      {
        this._exception = new AtlantisException(null, "ArtimGetMessagesResponseData", ex.Message, TargetedMessagesXml.ToString());
        _success = false;
      }
    }

    public ArtimGetMessagesResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
      _success = false;
    }

    public ArtimGetMessagesResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData, "ArtimGetMessagesResponseData", exception.Message, requestData.ToXML());
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

    public class TargetedMessages
    {
      public string AppID { get; set; }
      public string InteractionPoint { get; set; }
      public List<Message> Messages { get; set; }
      public int ResultCode { get; set; }
      public string SessionID { get; set; }
      public TargetedMessages()
      {
        Messages = new List<Message>();
      }
    }

    public class Message
    {
      public string DiscountCode { get; set; }
      public string DiscountType { get; set; }
      public string MessageID { get; set; }
      public string MessageSequence { get; set; }
      public string MessageUID { get; set; }
    }

  }
}
