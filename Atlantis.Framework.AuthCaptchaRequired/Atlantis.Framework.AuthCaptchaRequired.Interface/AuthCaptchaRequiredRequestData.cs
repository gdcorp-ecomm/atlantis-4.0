using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthCaptchaRequired.Interface
{
  public class AuthCaptchaRequiredRequestData: RequestData
  {

    public string IPAddress { get; set; }

    public AuthCaptchaRequiredRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string ipAddress)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      IPAddress = ipAddress;
      RequestTimeout = TimeSpan.FromSeconds(6);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
