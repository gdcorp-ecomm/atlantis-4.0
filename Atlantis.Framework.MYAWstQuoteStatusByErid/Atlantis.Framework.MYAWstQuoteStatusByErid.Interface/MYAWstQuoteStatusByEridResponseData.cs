using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MYAWstQuoteStatusByErid.Interface
{
  public class MYAWstQuoteStatusByEridResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;
    private QuoteResponse _quoteResponse = null;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public QuoteResponse QuoteResponse
    {
      get { return _quoteResponse; }
    }

    public MYAWstQuoteStatusByEridResponseData(string xml)
    {
      QuoteResponse quoteResponse = new QuoteResponse();
      quoteResponse.IsSuccess = false;

      XmlDocument xdoc = new XmlDocument();
      xdoc.LoadXml(xml);

      XmlNodeList quoteResponseNodes = xdoc.SelectNodes("QuoteResponse");

      if (quoteResponseNodes != null)
      {
        XmlNode successNode = quoteResponseNodes[0].FirstChild;
        XmlNode quoteOrErrorNodes = quoteResponseNodes[0].LastChild;

        if (quoteOrErrorNodes.LocalName == "Quote")
        {
          quoteResponse.Id = quoteOrErrorNodes.Attributes["Id"].Value;
          quoteResponse.StatusId = quoteOrErrorNodes.Attributes["StatusId"].Value;
          quoteResponse.StatusDescription = quoteOrErrorNodes.Attributes["StatusDescription"].Value;
        }
        else
        {
          quoteResponse.Error = quoteOrErrorNodes.FirstChild.FirstChild.Value;
        }

        quoteResponse.IsSuccess = Convert.ToBoolean(successNode.FirstChild.Value);
      }

      _resultXML = xml;
      _success = quoteResponse.IsSuccess;
      _quoteResponse = quoteResponse;
    }

     public MYAWstQuoteStatusByEridResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public MYAWstQuoteStatusByEridResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "MYAWstQuoteStatusByEridResponseData",
                                   exception.Message,
                                   requestData.ToXML());
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
