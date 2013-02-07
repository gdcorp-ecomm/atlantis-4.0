using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ResourceCountByPaymentProfile.Interface
{
  public class ResourceCountByPaymentProfileResponseData : IResponseData
  {
    private AtlantisException _atlException = null;

    public ResourceCountByPaymentProfileResponseData(int numberOfRecords)
    {
      IsSuccess = true;
      TotalRecords = numberOfRecords;
    }

    public ResourceCountByPaymentProfileResponseData(AtlantisException exAtlantis)
    {
      IsSuccess = false;
      _atlException = exAtlantis;
    }

    public ResourceCountByPaymentProfileResponseData(RequestData oRequestData, Exception ex)
    {
      IsSuccess = false;
      _atlException = new AtlantisException(oRequestData, "ResourceCountByPaymentProfileResponseData", ex.Message, string.Empty);
    }

    public bool IsSuccess { get; private set; }

    public int TotalRecords { get; private set; }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _atlException;
    }

    public string ToXML()
    {
      return string.Empty;
    }

    #endregion
  }
}
