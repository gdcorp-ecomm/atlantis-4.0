using System;
using System.Collections.Generic;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetSmtpRelayInfo.Interface
{
  public class ECCGetSmtpRelayInfoResponseData : EccResponseDataBase<EccSmtpRelayInfo>
  {

    public List<EccSmtpRelayInfo> SmtpRelayInfo
    {
      get
      {
        if (Response != null)
        {
          if (Response.Item != null)
          {
            if (Response.Item.Results != null)
              return Response.Item.Results;
          }
        }
        return null;
      }
    }

    public ECCGetSmtpRelayInfoResponseData(string resultJson) : base(resultJson)
    {
    }

	  public ECCGetSmtpRelayInfoResponseData(AtlantisException atlantisException) :  base(atlantisException)
    {}

    public ECCGetSmtpRelayInfoResponseData(RequestData requestData, Exception exception) : base(requestData,exception)
    {
    }

  }
}
