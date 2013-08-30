using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Segmentation
{
  public static class ShopperSegmentations
  {
    public const string Nacent = "nacent";
    public const string ActiveBusiness = "activebusiness";
    public const string eComm = "ecomm";
    public const string WebPro = "webpro";
    public const string Domainer = "domainer";

    private static Dictionary<int, string> _segmentations;

    static ShopperSegmentations()
    {
      _segmentations = new Dictionary<int, string>
      {
          {100, Nacent},
          {101, Nacent},
          {102, ActiveBusiness},
          {103, eComm },
          {104, WebPro},
          {105, Domainer}
      };
    }

    internal static string SegmentationName(this int id)
    {
      string value;
      return _segmentations.TryGetValue(id, out value) ? value : Nacent;
    }
  }

}
