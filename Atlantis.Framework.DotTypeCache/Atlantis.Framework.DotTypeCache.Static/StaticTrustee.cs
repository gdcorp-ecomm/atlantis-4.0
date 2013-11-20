using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public class StaticTrustee : ITLDTrustee
  {
    public bool IsRequired { get { return false; } }
  }
}