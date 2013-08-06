using System.Globalization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Brand.Interface
{
  public class ProductLineNameRequestData : RequestData
  {
    public int ContextId { get; private set; }

    public ProductLineNameRequestData(int contextId)
    {
      ContextId = contextId;
    }

    public override string GetCacheMD5()
    {
      return ContextId.ToString(CultureInfo.InvariantCulture);
    }

    public override string ToXML()
    {
      return "<productLineNames />";
    }
  }
}