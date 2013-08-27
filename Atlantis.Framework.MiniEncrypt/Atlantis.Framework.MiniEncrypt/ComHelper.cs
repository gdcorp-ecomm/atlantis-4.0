using System.Runtime.InteropServices;

namespace Atlantis.Framework.MiniEncrypt
{
  internal static class ComHelper
  {
    public static void SafeRelease(this object comObject)
    {
      if (comObject != null)
      {
        Marshal.ReleaseComObject(comObject);
        comObject = null;
      }
    }
   
  }
}
