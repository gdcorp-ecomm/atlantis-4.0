
namespace Atlantis.Framework.Providers.SplitTesting
{
  public class SplitTestingHelper
  {
    public static bool GetOverrideSide(int splitTestId, string o, out string overrideSide)
    {
      overrideSide = string.Empty;
      bool retVal = false;
      if (o != null)
      {
        string idStr = splitTestId.ToString();
        string[] tokens = o.Split(new [] {'.', '|'});
        for (int i = 0; i < tokens.Length; i += 2)
        {
          if (idStr.Equals(tokens[i]) && i + 1 < tokens.Length)
          {
            overrideSide = tokens[i + 1].ToUpperInvariant();
            retVal = true;
            if (string.IsNullOrEmpty(overrideSide))
            {
              overrideSide = string.Empty;
              retVal = false;
            }
            break;
          }
        }
      }
      return retVal;
    }


  }
}
