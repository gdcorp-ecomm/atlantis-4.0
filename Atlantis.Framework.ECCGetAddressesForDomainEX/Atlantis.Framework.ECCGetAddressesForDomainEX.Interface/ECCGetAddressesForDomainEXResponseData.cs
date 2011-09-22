using System;
using System.Collections.Generic;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetAddressesForDomainEX.Interface
{
  public class ECCGetAddressesForDomainEXResponseData : EccResponseDataBase<List<EccEmailItemEX>>
  {
    public ECCGetAddressesForDomainEXResponseData(string jsonResponse) : base(jsonResponse)
    {
    }

    public ECCGetAddressesForDomainEXResponseData(RequestData requestData, Exception ex) : base(requestData, ex)
    {
    }
  }
}
