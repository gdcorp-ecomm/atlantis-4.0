namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface IDomainProductLookup
  {
    int Years { get; set; }
    int DomainCount { get; set; }
    string TldPhaseCode { get; set; }
    TLDProductTypes ProductType { get; set; }
    int? PriceTierId { get; set; }
    int? RegistryId { get; set; }
  }
}
