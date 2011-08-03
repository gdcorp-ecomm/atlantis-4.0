using System;
using System.IO;
using System.Xml.XPath;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OrionGetAccountsByUID.Interface
{
  public class OrionGetAccountsByUIDResponseData : IResponseData
  {
    private readonly string _resultXML;
    AtlantisException _exception;

    public bool IsSuccess { get; private set; }

    public OrionGetAccountsByUIDResponseData(string xml, RequestData requestData)
    {
      _resultXML = xml;
      IsSuccess = ParseResponse(xml, requestData);

    }

    public OrionGetAccountsByUIDResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public OrionGetAccountsByUIDResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                         "OrionGetAccountsByUIDResponseData",
                                         exception.Message,
                                         requestData.ToXML());
    }

    bool ParseResponse(string xml, RequestData requestData)
    {
      bool result = false;
      try
      {
        XPathNavigator nav = new XPathDocument(new StringReader(xml)).CreateNavigator();
        XPathNodeIterator node = nav.Select("//GetAccountListByAccountUidResponse/GetAccountListByAccountUidResult");
        node.MoveNext();
        result = node.Current.Value == "0";
      }
      catch (Exception ex)
      {
        _exception = new AtlantisException(requestData, "OrionGetAccountsByUID.ParseResponse", ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
      }

      return result;
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
