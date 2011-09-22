using System;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetCalendarPlans.Interface
{
  public class ECCGetCalendarPlansResponseData : EccResponseDataBase<EccCalendarPlan>
  {
    public ECCGetCalendarPlansResponseData(string jsonResponse) : base (jsonResponse)
    {
    }

    public ECCGetCalendarPlansResponseData(RequestData requestData, Exception ex) : base(requestData, ex)
    {
    }
  }
}
