using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BillingResourceId.Interface
{
  public class BillingResourceIdResponseData : IResponseData
  {
    private AtlantisException AtlantisException { get; set; }
    private string Response { get; set; }

    public BillingResourceIdResponseData(string response)
    {
      this.Response = response;
    }

    public BillingResourceIdResponseData(AtlantisException atlantisException)
    {
      this.AtlantisException = atlantisException;
    }

    public BillingResourceIdResponseData(RequestData requestData, Exception exception)
    {
      this.AtlantisException = new AtlantisException(
        requestData,
        "BillingResourceIdResponseData",
        exception.Message, 
        requestData.ToXML());
    }

    public string ToXML()
    {
      return this.Response;
    }

    public AtlantisException GetException()
    {
      return this.AtlantisException;
    }
  }
}
