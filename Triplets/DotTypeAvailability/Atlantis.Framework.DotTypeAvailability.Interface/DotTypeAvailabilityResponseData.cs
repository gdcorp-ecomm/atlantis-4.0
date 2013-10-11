using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeAvailability.Interface
{
  public class DotTypeAvailabilityResponseData : IResponseData
  {
    private readonly bool _isSuccess;
    private readonly AtlantisException _exception;

    public DotTypeAvailabilityResponseData(IDictionary<string, ITldAvailability> tldAvailabilityList)
    {
      _exception = null;
      _isSuccess = true;
      TldAvailabilityList = tldAvailabilityList;
    }

    public DotTypeAvailabilityResponseData(AtlantisException exception)
    {
      _exception = exception;
      _isSuccess = false;
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    public IDictionary<string, ITldAvailability> TldAvailabilityList { get; private set; }
  }
}
