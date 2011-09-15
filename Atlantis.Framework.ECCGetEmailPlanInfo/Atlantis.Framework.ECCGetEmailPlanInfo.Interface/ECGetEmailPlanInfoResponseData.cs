using System;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetEmailPlanInfo.Interface
{
  public class EccGetEmailPlanInfoResponseData : EccResponseDataBase<EccEmailPlanDetails>
  {
    public EccEmailPlanDetails EmailPlanDetails
    {
      get
      {
        if (Response != null && Response.Item != null && Response.Item.Results != null && Response.Item.Results.Count > 0)
        {
          return Response.Item.Results[0];
        }
        return null;
      }
    }

    public EccGetEmailPlanInfoResponseData(string resultJson) : base(resultJson)
    {
    }
    
    public EccGetEmailPlanInfoResponseData(AtlantisException atlantisException) : base(atlantisException)
    {
    }

    public EccGetEmailPlanInfoResponseData(RequestData requestData, Exception exception) : base (requestData, exception)
    {
    }
  }
}
