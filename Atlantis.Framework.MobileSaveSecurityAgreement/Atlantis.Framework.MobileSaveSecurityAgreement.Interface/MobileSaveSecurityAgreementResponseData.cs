using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobileSaveSecurityAgreement.Interface
{
  public class MobileSaveSecurityAgreementResponseData : IResponseData
  {
    #region Members

    private AtlantisException _ex;

    #endregion

    #region Constructors

    public MobileSaveSecurityAgreementResponseData(bool successful)
    {
      Successful = successful;
    }

    public MobileSaveSecurityAgreementResponseData(AtlantisException ex)
    {
      _ex = ex;
    }

    public MobileSaveSecurityAgreementResponseData(RequestData requestData, Exception ex)
    {
      _ex = new AtlantisException(requestData, "MobileSaveSecurityAgreementResponseData", ex.Message, requestData.ToXML());
    }

    #endregion

    #region Properties

    public bool Successful { get; set; }

    #endregion

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion
  }
}
