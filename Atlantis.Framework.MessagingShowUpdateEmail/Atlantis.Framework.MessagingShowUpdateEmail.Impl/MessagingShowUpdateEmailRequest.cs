using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MessagingShowUpdateEmail.Impl.MessagingWbSvc;
using Atlantis.Framework.MessagingShowUpdateEmail.Interface;

namespace Atlantis.Framework.MessagingShowUpdateEmail.Impl
{
  public class MessagingShowUpdateEmailRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData oRequestData, ConfigElement config)
    {
      MessagingShowUpdateEmailResponseData responseData = null;
      bool shopperEmailUpdateInfo = false;
      try
      {
        MessagingShowUpdateEmailRequestData requestData = (MessagingShowUpdateEmailRequestData)oRequestData;
        using (MessagingWebService service = new MessagingWebService())
        {
          service.Url = ((WsConfigElement)config).WSURL;
          service.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          if (!string.IsNullOrEmpty(requestData.ShopperEmail))
          {
            string emailHash = HashEmail(requestData.ShopperEmail);
            shopperEmailUpdateInfo = service.ShowUpdateEmailMessage(emailHash, requestData.PrivateLabelId);
          }
          responseData = new MessagingShowUpdateEmailResponseData(shopperEmailUpdateInfo);
        }
      } 
      catch (AtlantisException exAtlantis)
      {
        responseData = new MessagingShowUpdateEmailResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new MessagingShowUpdateEmailResponseData(oRequestData, ex);
      }
      return responseData;
    }

   private string HashEmail(string email)
   {
     if (email == string.Empty)
     {
       return string.Empty;
     }

     SHA256Managed sha = new SHA256Managed();
     ASCIIEncoding ue = new ASCIIEncoding();

     byte[] hashValue = sha.ComputeHash(ue.GetBytes(email));
     byte[] s = new byte[64];

     int nLen = 32;

     int i;
     int nPos = nLen * 2;
     byte c;

     // encode byte pairs with mod and divide
     for (i = nLen - 1; i >= 0; i--)
     {
       c = hashValue[i];
       s[--nPos] = (byte)(c % 10 + 97);
       s[--nPos] = (byte)(c / 10 + 97);
     }

     return ue.GetString(s);
   }
  }
}
