using System;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetAutoResponder.Interface
{
  public class ECCGetAutoResponderResponseData : EccResponseDataBase<EccAutoResponderDetails>
  {
    public ECCGetAutoResponderResponseData(string jsonResponse) : base(jsonResponse)
    {
      
    }

    public ECCGetAutoResponderResponseData(RequestData requestData, Exception ex) : base(requestData, ex)
    {
    }

  }
}
