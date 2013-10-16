using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TldValidYearsSet : ITLDValidYearsSet
  {
    private static readonly ITLDValidYearsSet _noValidYears;
    
    private readonly HashSet<int> _validYears;
    private readonly int _minimumYears;
    private readonly int _maximumYears;

    static TldValidYearsSet()
    {
      _noValidYears = FromNothing();
    }

    public static ITLDValidYearsSet INVALIDSET
    {
      get { return _noValidYears; }
    }

    public static ITLDValidYearsSet FromPeriodElements(IEnumerable<XElement> periodElements)
    {
      return new TldValidYearsSet(periodElements);
    }

    public static ITLDValidYearsSet FromNothing()
    {
      return new TldValidYearsSet();
    }

    private TldValidYearsSet()
    {
      _validYears = new HashSet<int>();
    }

    private TldValidYearsSet(IEnumerable<XElement> periodElements)
    {
      _validYears = new HashSet<int>();
      foreach (XElement periodItem in periodElements)
      {
        if (periodItem.IsEnabled(false))
        {
          int years = (int)periodItem.PeriodYears(0d);
          if (years > 0)
          {
            _validYears.Add(years);
            if ((_minimumYears == 0) || (years < _minimumYears))
            {
              _minimumYears = years;
            }

            if (years > _maximumYears)
            {
              _maximumYears = years;
            }
          }
        }
      }
    }

    public bool IsValid(int years)
    {
      return _validYears.Contains(years);
    }

    public int Min
    {
      get { return _minimumYears; }
    }

    public int Max
    {
      get { return _maximumYears; }
    }
  }
}