using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Engine.Tests.MockTriplet
{
  public class ConfigTestResponseData : IResponseData
  {
    private AtlantisException _exception;

    public bool IsSuccess { get; set; }

    public ConfigTestResponseData()
    {
      IsSuccess = true;
    }

    public ConfigTestResponseData(AtlantisException exception)
    {
      IsSuccess = false;
      _exception = exception;
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
