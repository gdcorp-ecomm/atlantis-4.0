using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddItem.Interface
{
  public class AddItemResponseData : IResponseData
  {
    private readonly string _responseXml;
    private readonly AtlantisException _exception;

    public AddItemResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _exception = null;
    }

    public AddItemResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public AddItemResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(requestData, "AddItemResponseData", ex.Message, requestData.ToXML());
    }

    public bool IsSuccess
    {
      get { return _responseXml.IndexOf("success", StringComparison.OrdinalIgnoreCase) > -1; }
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _responseXml;
    }

    #endregion
  }
}
