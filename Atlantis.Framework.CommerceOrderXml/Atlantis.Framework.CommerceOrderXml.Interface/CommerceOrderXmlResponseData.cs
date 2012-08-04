using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CommerceOrderXml.Interface
{
  public class CommerceOrderXmlResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    public CommerceOrderXmlResponseData(string xml)
    {
      _resultXML = xml;
    }

    public CommerceOrderXmlResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public CommerceOrderXmlResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "MYAOrderDetailResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    public bool IsSuccess
    {
      get
      {
        XmlNode node = OrderXML.SelectSingleNode("ERRORS");
        if (node == null)
        {
          _success = true;
        }
        return _success;
      }
    }

    private XmlDocument _orderXML = null;
    public XmlDocument OrderXML
    {
      get
      {
        if (_orderXML == null)
        {
          _orderXML = new XmlDocument();
          _orderXML.LoadXml(_resultXML);
        }
        return _orderXML;
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

  }
}