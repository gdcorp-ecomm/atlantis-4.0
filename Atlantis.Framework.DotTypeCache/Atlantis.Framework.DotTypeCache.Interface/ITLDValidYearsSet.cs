namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface ITLDValidYearsSet
  {
    int Min { get; }

    int Max { get; }

    bool IsValid(int years);
  }
}
