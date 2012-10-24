using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommOrderItemCommonName.Interface
{
  public class EcommOrderItemCommonNameResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public string CommonName { get; private set; }

    public EcommOrderItemCommonNameResponseData(string commonName)
    {
      CommonName = commonName;
    }

    public EcommOrderItemCommonNameResponseData(RequestData requestData, Exception exception)
    {
      // These types of error messages are "noise" - don't log, just swallow.  Log others.
      if (!(exception.Message.Contains("No SQL procedure") || exception.Message.Contains("Could not find stored procedure")))
      {
        _exception = new AtlantisException(requestData
          , "EcommOrderItemCommonNameResponseData"
          , exception.Message
          , requestData.ToXML());
      }
    }

    #region IResponseData Members

    public string ToXML()
    {
      _resultXML = string.Format("<commonName>{0}</commonName>", CommonName);
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
