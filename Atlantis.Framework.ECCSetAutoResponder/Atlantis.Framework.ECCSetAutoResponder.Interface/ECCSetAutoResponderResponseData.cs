using System;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCSetAutoResponder.Interface
{
  public class ECCSetAutoResponderResponseData : EccResponseDataBase<object>
  {

    public ECCSetAutoResponderResponseData(string resultJson) :base(resultJson)
    {
    }

    public ECCSetAutoResponderResponseData(RequestData requestData, Exception ex) : base(requestData,ex)
    {
    }
  }
}
