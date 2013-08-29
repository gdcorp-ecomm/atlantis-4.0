using System.Collections.Generic;

namespace Atlantis.Framework.CH.Segmentation
{
  public static class ShopperSegmentations
  {
    public const string Nacent = "Nacent";
    public const string ActiveBusiness = "ActiveBusiness";
    public const string eComm = "eComm";
    public const string WebPro = "WebPro";
    public const string Domainer = "Domainer";

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

    public static string SegmentationName(this int id)
    {
      string value;
      return _segmentations.TryGetValue(id, out value) ? value : Nacent;
    }
  }

}
