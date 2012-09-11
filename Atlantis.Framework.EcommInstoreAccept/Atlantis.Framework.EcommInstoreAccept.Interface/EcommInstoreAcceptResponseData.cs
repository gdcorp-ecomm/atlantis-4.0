using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.EcommInstoreAccept.Interface
{
  public class EcommInstoreAcceptResponseData : IResponseData
  {
    private AtlantisException _exception = null;

    public InstoreAcceptResult Result { get; set; }
    public int ResultCode { get; set; }
    public string ResultMessage { get; set; }

    private void SetResult()
    {
      Result = InstoreAcceptResult.UnknownResult;
      if (Enum.IsDefined(typeof(InstoreAcceptResult), ResultCode))
      {
        Result = (InstoreAcceptResult)ResultCode;
      }
    }

    public EcommInstoreAcceptResponseData(int resultCode, string resultMessage)
    {
      ResultCode = resultCode;
      ResultMessage = resultMessage;
      SetResult();
    }

    public EcommInstoreAcceptResponseData(RequestData request, Exception ex)
    {
      ResultCode = -1;
      ResultMessage = string.Empty;
      SetResult();
      _exception = new AtlantisException(request, "EcommInstoreAccceptResponseData.ctor", ex.Message + ex.StackTrace, request.ShopperID, ex);
    }

    public string ToXML()
    {
      return new XElement("EcommInstoreAccept", new XAttribute("resultcode",ResultCode)).ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
