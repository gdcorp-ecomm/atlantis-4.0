using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RegGetCAProfileData.Interface
{
  public class RegGetCAProfileDataResponseData : IResponseData
  {
    private string _responseXML;
    private AtlantisException _atlException;

    public RegGetCAProfileDataResponseData(string responseXML)
    {
      _responseXML = responseXML;
    }

    public RegGetCAProfileDataResponseData(AtlantisException exAtlantis)
    {
      _responseXML = "";
      _atlException = exAtlantis;
    }

    public RegGetCAProfileDataResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      _responseXML = sResponseXML;
      _atlException = new AtlantisException(oRequestData, "RegGetCAProfileDataResponseData", ex.Message, string.Empty);
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _atlException;
    }

    public string ToXML()
    {
      return _responseXML;
    }

    #endregion
  }
}
