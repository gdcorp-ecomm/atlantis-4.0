using System;
using Atlantis.Framework.Interface;
using System.Xml;

namespace Atlantis.Framework.GuestbookSurveyResults.Interface
{
  public class GuestbookSurveyResultsResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private bool _isSuccess = false;
    private XmlNode _responseNode = null;

    public GuestbookSurveyResultsResponseData(XmlNode responseNode, RequestData requestData)
    {
      _responseNode = responseNode;
      if (_responseNode == null)
      {
        _isSuccess = false;
        _exception = new AtlantisException(requestData, "GuestbookSurveyResultsResponseData", "Response node was null.", requestData.ToXML());
      }
      else if (_responseNode.ChildNodes.Count == 0)
      {
        _isSuccess = false;
        _exception = new AtlantisException(requestData, "GuestbookSurveyResultsResponseData", "Response node was empty because of error or no data.", requestData.ToXML());
      }
      else
      {
        _isSuccess = true;
      }
    }

    public GuestbookSurveyResultsResponseData(XmlNode responseNode, AtlantisException exception)
    {
      _responseNode = responseNode;
      _exception = exception;
    }

    public GuestbookSurveyResultsResponseData(XmlNode responseNode, RequestData requestData, Exception exception)
    {
      _responseNode = responseNode;
      _exception = new AtlantisException(requestData, "GuestbookSurveyResultsResponseData", exception.Message, requestData.ToXML(), exception);
    }

    public bool IsSuccess
    {
      get
      {
        return _isSuccess;
      }
    }

    public XmlNode ResponseNode
    {
      get
      {
        return _responseNode;
      }
    }

    #region IResponseData Members

    public string ToXML()
    {
      string result = string.Empty;
      if (_responseNode != null)
      {
        result = _responseNode.OuterXml;
      }
      return result;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
