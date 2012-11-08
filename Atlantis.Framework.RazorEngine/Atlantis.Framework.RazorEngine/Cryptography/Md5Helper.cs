using System.Security.Cryptography;
using System.Text;

namespace Atlantis.Framework.RazorEngine.Cryptography
{
  internal static class Md5Helper
  {
    private static readonly MD5 _md5HashManager = MD5.Create();

    internal static string GenerateHash(string key)
    {
      byte[] inputBytes = Encoding.ASCII.GetBytes(key);
      byte[] hashBytes = _md5HashManager.ComputeHash(inputBytes);

      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < hashBytes.Length; i++)
      {
        sb.Append(hashBytes[i].ToString("X2"));
      }

      return sb.ToString();
    }
  }
}
