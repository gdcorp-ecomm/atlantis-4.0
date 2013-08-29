using Atlantis.Framework.Interface;
using System.Globalization;

namespace Atlantis.Framework.Products.Interface
{
  public class ProductNamesRequestData : RequestData
  {
    public string FullLanguage { get; private set; }
    public int NonUnifiedPfid { get; private set; }

    public ProductNamesRequestData(string fullLangauge, int nonUnifiedPfid)
    {
      FullLanguage = (fullLangauge != null) ? fullLangauge.ToLowerInvariant() : string.Empty;
      NonUnifiedPfid = nonUnifiedPfid;
    }

    public override string GetCacheMD5()
    {
      return string.Concat(FullLanguage, "-", NonUnifiedPfid.ToString(CultureInfo.InvariantCulture));
    }
  }
}
