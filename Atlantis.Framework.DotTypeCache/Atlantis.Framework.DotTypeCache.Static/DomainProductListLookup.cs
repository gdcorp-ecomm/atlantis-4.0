using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public class DomainProductListLookup : IDomainProductListLookup
  {
    public int[] Years { get; set; }
    public int DomainCount { get; set; }
    public string TldPhaseCode { get; set; }
    public TLDProductTypes ProductType { get; set; }
    public int? PriceTierId { get; set; }
    public int? RegistryId { get; set; }
  }
}
