using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaMirageStatus.Interface
{
  public class MyaMirageStatusResponseData : IResponseData
  {
    private AtlantisException _exception;
    private DateTime? _lastMirageBuild = null;

    public MyaMirageStatusResponseData(DateTime? lastMirageBuild)
    {
      _lastMirageBuild = lastMirageBuild;
    }

    public bool IsMirageCurrent
    {
      get { return _lastMirageBuild == null; }
    }

    public DateTime LastMirageBuild
    {
      get
      {
        if (_lastMirageBuild.HasValue)
        {
          return _lastMirageBuild.Value;
        }
        else
        {
          return DateTime.MaxValue;
        }
      }
    }

    public MyaMirageStatusResponseData(Exception ex, RequestData request)
    {
      _exception = new AtlantisException(request, "MyaMirageStatusResponse", ex.Message + ex.StackTrace, request.ShopperID);
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
