using System;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommClientCertCheck.Interface
{
  public class EcommClientCertCheckResponeData : IResponseData
  {
    private AtlantisException AtlantisException { get; set; }

    public bool IsAuthorized { get; private set; }

    public bool IsSuccess { get; private set; }

    public EcommClientCertCheckResponeData(bool isAuthorized)
    {
      IsAuthorized = isAuthorized;
      IsSuccess = true;
    }

    public EcommClientCertCheckResponeData(EcommClientCertCheckRequestData requestData, Exception ex)
    {
      IsSuccess = false;
      AtlantisException = new AtlantisException(requestData,
                                                MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                "Error calling EcommClientCertCheck service.",
                                                ex.StackTrace,
                                                ex);
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
