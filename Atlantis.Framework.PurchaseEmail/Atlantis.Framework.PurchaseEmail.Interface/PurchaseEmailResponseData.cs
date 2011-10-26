using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MessagingProcess.Interface;

namespace Atlantis.Framework.PurchaseEmail.Interface
{
  public class PurchaseEmailResponseData : IResponseData
  {
    List<MessagingProcessResponseData> _messageResponses;
    List<Exception> _exceptions;

    public PurchaseEmailResponseData()
    {
      _messageResponses = new List<MessagingProcessResponseData>();
      _exceptions = new List<Exception>();
    }

    public List<MessagingProcessResponseData> MessageResponses
    {
      get { return _messageResponses; }
    }

    public List<Exception> Exceptions
    {
      get { return _exceptions; }
    }

    public void AddMessageResponse(MessagingProcessResponseData response)
    {
      _messageResponses.Add(response);
    }

    public void AddException(Exception ex)
    {
      _exceptions.Add(ex);
    }

    public bool IsSuccess
    {
      get
      {
        bool result = true;
        if (_exceptions.Count > 0)
        {
          result = false;
        }
        else
        {
          foreach (MessagingProcessResponseData response in _messageResponses)
          {
            if (!response.IsSuccess)
            {
              result = false;
              break;
            }
          }
        }
        return result;
      }
    }

    #region IResponseData Members

    public string ToXML()
    {
      // 109 = size of response XML not including MessageResponse or Errors
      // 55 = size of XML for message response
      StringBuilder stringBuilder = new StringBuilder(109 + 55 * _messageResponses.Count);
      XmlWriter xmlWriter = XmlWriter.Create(new StringWriter(stringBuilder));

      xmlWriter.WriteStartElement("PurchaseEmailResponseData");
      {
        xmlWriter.WriteStartElement("Status");
        xmlWriter.WriteValue(_exceptions.Count == 0 ? "SUCCESS" : "FAILURE");
        xmlWriter.WriteEndElement();

        if (_messageResponses.Count > 0)
        {
          xmlWriter.WriteStartElement("MessageResponses");
          foreach (MessagingProcessResponseData messageData in _messageResponses)
          {
            xmlWriter.WriteRaw(messageData.ToXML());
          }
          xmlWriter.WriteEndElement();
        }

        if (_exceptions.Count > 0)
        {
          xmlWriter.WriteStartElement("Errors");
          foreach (Exception ex in _exceptions)
          {
            xmlWriter.WriteStartElement("Error");
            {
              xmlWriter.WriteStartElement("Message");
              xmlWriter.WriteValue(ex.Message);
              xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
          }
          xmlWriter.WriteEndElement();
        }
      }
      xmlWriter.WriteEndElement();

      xmlWriter.Flush();
      return stringBuilder.ToString();
    }

    public AtlantisException GetException()
    {
      return null;
    }

    #endregion
  }
}
