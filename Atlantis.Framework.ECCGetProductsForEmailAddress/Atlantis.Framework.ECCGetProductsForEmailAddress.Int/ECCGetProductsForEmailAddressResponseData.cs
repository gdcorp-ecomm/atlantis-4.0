using System;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetProductsForEmailAddress.Interface
{
  public class ECCGetProductsForEmailAddressResponseData : EccResponseDataBase<object>
  {
   
    public ECCGetProductsForEmailAddressResponseData(string jsonResponse): base(jsonResponse)
    {
    }

    public ECCGetProductsForEmailAddressResponseData(RequestData requestData, Exception ex) : base (requestData,ex)
    {
    }

  }
}
