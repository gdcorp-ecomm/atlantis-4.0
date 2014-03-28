namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface ITLDTrustee
  {
    bool IsRequired { get; }
    int  TrusteeVendorId { get; }
  }
}
