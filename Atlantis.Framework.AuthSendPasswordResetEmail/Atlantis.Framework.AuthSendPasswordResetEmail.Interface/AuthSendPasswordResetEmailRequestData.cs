using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthSendPasswordResetEmail.Interface
{
  public class AuthSendPasswordResetEmailRequestData: RequestData
  {
    public string LocalizationCode { get; private set; }
    public int PrivateLabelID { get; private set; }

    public AuthSendPasswordResetEmailRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId, string localizationCode = "") : 
      base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      LocalizationCode = localizationCode;
      PrivateLabelID = privateLabelId;
      RequestTimeout = TimeSpan.FromSeconds(6);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
