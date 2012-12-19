using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoRedemptionCode.Interface
{
  public class PromoRedemptionCodeResponseData : IResponseData
  {
    private AtlantisException _exception { get; set; }

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public RedemptionCodeStatus CodeStatus { get; private set; }

    public PromoRedemptionCodeResponseData(RedemptionCodeStatus codeStatus)
    {
      CodeStatus = codeStatus;
    }

    public PromoRedemptionCodeResponseData(AtlantisException aex)
    {
      _exception = aex;
    }

    public PromoRedemptionCodeResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message, ex.StackTrace, ex);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return string.Empty;
    }
  }
}