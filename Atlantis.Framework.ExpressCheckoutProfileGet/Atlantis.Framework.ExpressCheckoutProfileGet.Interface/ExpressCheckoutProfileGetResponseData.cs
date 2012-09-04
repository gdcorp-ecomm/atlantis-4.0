using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ExpressCheckoutProfileGet.Interface
{
  public class ExpressCheckoutProfileGetResponseData : IResponseData
  {
    private bool _success = false;
    private AtlantisException _exception = null;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public int ProfileId { get; set; }

    public ExpressCheckoutProfileGetResponseData(int profileId)
    {
      _success = true;
      ProfileId = profileId;
    }

    public ExpressCheckoutProfileGetResponseData(AtlantisException exAtlantis)
    {
      _exception = exAtlantis;
      _success = false;
    }

    public ExpressCheckoutProfileGetResponseData(RequestData oRequestData, Exception ex)
    {
      _exception = new AtlantisException(oRequestData, "ExpressCheckoutProfileGetResponseData", ex.Message, string.Empty);
      _success = false;
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

  }
}
