using System;
using System.Collections.Generic;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetOFFPlanInfo.Interface
{
  public class ECCGetOFFPlanInfoResponseData : EccResponseDataBase<EccOFFPlanDetails>
  {

    public List<EccOFFPlanDetails> OFFPlanDetails
    {
      get
      {
        if (Response != null)
        {
          if (Response.Item != null)
          {
            if (Response.Item.Results != null)
              if (Response.Item.Results.Count > 0) return Response.Item.Results;
          }
        }
        return null;
      }
    }

    public ECCGetOFFPlanInfoResponseData(string resultJson)
      : base(resultJson)
    {
    }

    public ECCGetOFFPlanInfoResponseData(AtlantisException atlantisException)
      : base(atlantisException)
    {
    }

    public ECCGetOFFPlanInfoResponseData(RequestData requestData, Exception exception)
      : base(requestData, exception)
    {
    }

  }
}
