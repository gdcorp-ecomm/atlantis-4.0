using System;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobileApplePush.Interface
{
  public class MobileApplePushResponseData : IResponseData
  {
    private AtlantisException AtlantisException { get; set; }

    public bool IsSuccess { get; private set; }

    public MobileApplePushResponseData(bool success)
    {
      IsSuccess = success;
    }

    public MobileApplePushResponseData(MobileApplePushRequestData requestData, Exception ex)
    {
      IsSuccess = false;
      AtlantisException = new AtlantisException(requestData,
                                                MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                ex.Message,
                                                ex.StackTrace);
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return AtlantisException;
    }
  }
}
