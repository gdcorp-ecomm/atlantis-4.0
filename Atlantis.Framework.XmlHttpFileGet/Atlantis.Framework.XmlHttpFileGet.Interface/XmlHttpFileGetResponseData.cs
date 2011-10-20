using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.XmlHttpFileGet.Interface
{
  public class XmlHttpFileGetResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private string _resultXml = string.Empty;
    private readonly bool _success;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public DateTime RetrieveDate { get; private set; }

    public XmlHttpFileGetResponseData(XDocument xmlDoc, DateTime retrieveDate)
    {
      _success = true;
      Response = xmlDoc;
      RetrieveDate = retrieveDate;
    }

    public XmlHttpFileGetResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public XmlHttpFileGetResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                         "XmlHttpFileGetResponseData",
                                         exception.Message,
                                         requestData.ToXML());
    }

    public XDocument Response { get; private set; }

    #region IResponseData Members

    public string ToXML()
    {
      if (string.IsNullOrEmpty(_resultXml))
      {
        if (Response != null)
        {
          _resultXml = Response.ToString();
        }
      }

      return _resultXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}