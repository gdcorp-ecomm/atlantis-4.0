using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Atlantis.Framework.MobileAndroidPush.Impl
{
  internal static class GoogleCertificatePolicy
  {
    public static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
    {
      // We do not need to validate google certificates
      // The push certificate is issues from google.com but the host name for the push service is different
      return true;
    }
  }
}
