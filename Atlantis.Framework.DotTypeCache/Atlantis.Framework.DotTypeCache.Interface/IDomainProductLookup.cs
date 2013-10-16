namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface IDomainProductLookup
  {
    int Years { get; }

    int DomainCount { get; }

    LaunchPhases TldPhase { get; }

    TLDProductTypes ProductType { get; }

    int? PriceTierId { get; }

    int? RegistryId { get; }
  }
}
