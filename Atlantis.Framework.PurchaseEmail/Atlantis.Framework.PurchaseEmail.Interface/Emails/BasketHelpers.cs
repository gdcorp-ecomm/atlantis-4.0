
namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal static class BasketHelpers
  {
    public static void GetDomainParts(string domain, out string sld, out string tld)
    {
      if (string.IsNullOrEmpty(domain))
      {
        sld = string.Empty;
        tld = string.Empty;
        return;
      }
      sld = string.Empty;
      tld = string.Empty;
      string currentDomain = domain;
      int dotPoint = currentDomain.IndexOf(".");
      if (dotPoint > -1)
      {
        sld = currentDomain.Substring(0, dotPoint);
        tld = currentDomain.Substring(dotPoint + 1);
      }
    }
  }
}
