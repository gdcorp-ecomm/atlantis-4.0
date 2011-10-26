using System;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCSetOFFAccount.Interface
{
  public class ECCSetOFFAccountResponseData : EccResponseDataBase<object> 
  {
    public ECCSetOFFAccountResponseData(string jsonResponse) : base(jsonResponse)
    {}

    public ECCSetOFFAccountResponseData(RequestData requestData, Exception ex) : base(requestData, ex)
    {
    }
  }
}
