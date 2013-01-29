namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface ITLDValidYearsSet
  {
    bool IsValid(int years);
    int Min { get; }
    int Max { get; }
  }
}
