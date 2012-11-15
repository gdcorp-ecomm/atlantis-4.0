using System;
using System.IO;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OAuthValidateClientId.Interface
{
  public class OAuthValidateClientIdResponseData : IResponseData
  {
    private readonly AtlantisException _aex;

    public bool IsValidClientId { get; private set; }
    public string ClientPalmsId { get; private set; }
    public string ErrorCode { get; private set; }
    public int PalmsReturnValue { get; private set; }

    public bool IsSuccess
    {
      get { return _aex == null; }
    }

    #region Constructors
    public OAuthValidateClientIdResponseData(bool isValidClientId, string clientPalmsId, string errorCode, int palmsReturnValue)
    {
      IsValidClientId = isValidClientId;
      ClientPalmsId = clientPalmsId;
      ErrorCode = errorCode;
      PalmsReturnValue = palmsReturnValue;
    }

    public OAuthValidateClientIdResponseData(AtlantisException aex, string errorCode)
    {
      _aex = aex;
      ErrorCode = errorCode;
    }

    public OAuthValidateClientIdResponseData(RequestData requestData, Exception ex, string errorCode)
    {
      _aex = new AtlantisException(requestData, "OAuthValidateClientIdResponseData", ex.Message, ex.StackTrace);
      ErrorCode = errorCode;
    }
    #endregion

    #region Member Vars
    public string ToXML()
    {
      return string.Format("<IsValidClientId>{0}</IsValidClientId>", IsValidClientId.ToString());
    }

    public AtlantisException GetException()
    {
      return _aex;
    }
    #endregion
  }
}
