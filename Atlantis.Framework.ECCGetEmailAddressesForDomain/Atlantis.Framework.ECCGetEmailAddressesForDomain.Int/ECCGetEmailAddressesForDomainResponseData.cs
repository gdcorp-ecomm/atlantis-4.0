using System;
using System.Collections.Generic;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetEmailAddressesForDomain.Interface
{
  public class ECCGetEmailAddressesForDomainResponseData : EccResponseDataBase<List<String>>
  {
    public ECCGetEmailAddressesForDomainResponseData(string jsonResponse):base(jsonResponse)
    {
    }

    public ECCGetEmailAddressesForDomainResponseData(RequestData requestData, Exception ex) : base(requestData,ex)
    {
    }
  }
}
