using System;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetOFFAccountsForShopper.Interface
{
  public class ECCGetOFFAccountsForShopperResponseData : EccResponseDataBase<EccOFFAccountDetails>
  {

    public ECCGetOFFAccountsForShopperResponseData(string jsonResponse)
      : base(jsonResponse)
    {
    }

    public ECCGetOFFAccountsForShopperResponseData(RequestData requestData, Exception ex)
      : base(requestData, ex)
    {
    }
  }
}
