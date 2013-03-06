namespace Atlantis.Framework.Testing.MockProviders
{
  internal static class KnownPrivateLabelIds
  {
    public const int GoDaddy = 1;
    public const int WildWestDomains = 1387;
    public const int BlueRazor = 2;

    public static int GetContextId(int privateLabelId)
    {
      int result = 6;
      switch (privateLabelId)
      {
        case KnownPrivateLabelIds.GoDaddy:
          result = 1;
          break;
        case KnownPrivateLabelIds.BlueRazor:
          result = 5;
          break;
        case KnownPrivateLabelIds.WildWestDomains:
          result = 2;
          break;
      }
      return result;
    }
  }
}
