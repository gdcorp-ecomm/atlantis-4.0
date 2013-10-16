using System;
using System.Text;

namespace Atlantis.Framework.Sso.Interface
{
  public static class DecryptionHelper
  {
    public static byte[] Base64UrlDecodeToBytes(string s)
    {
      // Deal with differences between base64 and base64url encodings:
      s = s.Replace('-', '+');
      s = s.Replace('_', '/');
      s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');

      return Convert.FromBase64String(s);
    }

    public static string Base64UrlDecodeToString(string s)
    {
      return Encoding.UTF8.GetString(Base64UrlDecodeToBytes(s));
    }
  }
}
