using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CommerceOrderXml.Interface
{
  public class CommerceOrderXmlResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public CommerceOrderXmlResponseData(string xml)
    {
      _resultXML = xml;
      _success = true;
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