namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface IDotTypeProvider
  {
    IDotTypeInfo InvalidDotType { get; }
    IDotTypeInfo GetDotTypeInfo(string dotType);
    bool HasDotTypeInfo(string dotType);

    ITLDDataImpl GetTLDDataForInvalid { get; }
    ITLDDataImpl GetTLDDataForRegistration { get; }
    ITLDDataImpl GetTLDDataForTransfer { get; }
    ITLDDataImpl GetTLDDataForBulk { get; }
    ITLDDataImpl GetTLDDataForBulkTransfer { get; }
  }
}
