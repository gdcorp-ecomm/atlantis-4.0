using System;
using System.Collections.Generic;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetAddressesForPlanEX.Interface
{
  public class ECCGetAddressesForPlanEXResponseData : EccResponseDataBase<List<EccEmailItemEX>>
  {
    public ECCGetAddressesForPlanEXResponseData(string jsonResponse) : base(jsonResponse)
    {
    }

    public ECCGetAddressesForPlanEXResponseData(RequestData requestData, Exception ex) : base(requestData, ex)
    {
    }
  }
  
}
