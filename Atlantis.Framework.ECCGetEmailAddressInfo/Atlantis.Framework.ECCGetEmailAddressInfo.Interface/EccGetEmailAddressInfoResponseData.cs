using System;
using System.Collections.Generic;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EccGetEmailAddressInfo.Interface
{
  public class EccGetEmailAddressInfoResponseData: EccResponseDataBase<EccEmailAddressDetails>
  {
   
    public List<EccEmailAddressDetails> Results
    {
      get { return Response.Item.Results; }
    }

    public EccGetEmailAddressInfoResponseData(string resultJson) : base (resultJson)
    {
    }

    public EccGetEmailAddressInfoResponseData(AtlantisException atlantisException) : base (atlantisException)
    {
    }

    public EccGetEmailAddressInfoResponseData(RequestData requestData, Exception exception) : base(requestData, exception)
    {
    }
  }
}
