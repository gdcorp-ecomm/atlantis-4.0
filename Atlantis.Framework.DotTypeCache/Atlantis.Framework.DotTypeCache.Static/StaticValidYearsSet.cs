using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public class StaticValidYearsSet : ITLDValidYearsSet
  {
    private readonly int _min;
    private readonly int _max;

    public StaticValidYearsSet(int min, int max)
    {
      _min = min;
      _max = max;
    }

    public bool IsValid(int years)
    {
      return (years >= _min && years <= _max);
    }

    public int Min
    {
      get { return _min; }
    }

    public int Max
    {
      get { return _max; }
    }
  }
}
