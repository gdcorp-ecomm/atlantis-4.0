namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface IDotTypeProvider
  {
    IDotTypeInfo InvalidDotType { get; }
    IDotTypeInfo GetDotTypeInfo(string dotType);
    bool HasDotTypeInfo(string dotType);
  }
}
