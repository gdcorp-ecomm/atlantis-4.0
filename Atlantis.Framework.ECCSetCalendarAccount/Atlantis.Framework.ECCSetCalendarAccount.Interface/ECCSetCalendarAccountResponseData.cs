using System;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCSetCalendarAccount.Interface
{
  public class ECCSetCalendarAccountResponseData : EccResponseDataBase<object>
  {
    public ECCSetCalendarAccountResponseData(string resultJson) : base(resultJson)
    {}

    public ECCSetCalendarAccountResponseData(RequestData requestData, Exception ex) : base(requestData, ex)
    {}
  }
}
