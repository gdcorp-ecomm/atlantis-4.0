using System;
using System.Collections.Generic;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetAddressesForShopperEX.Interface
{
  public class ECCGetAddressesForShopperEXResponseData : EccResponseDataBase<List<EccEmailItemEX>>
  {
    public ECCGetAddressesForShopperEXResponseData(string jsonResponse) : base(jsonResponse)
    {
    }

    public ECCGetAddressesForShopperEXResponseData(RequestData requestData, Exception ex) :base(requestData, ex)
    {
    }
  }
}
