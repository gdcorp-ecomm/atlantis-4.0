using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TldValidYearsSet
  {
    private static TldValidYearsSet _noValidYears;

    static TldValidYearsSet()
    {
      _noValidYears = TldValidYearsSet.FromNothing();
    }

    public static TldValidYearsSet INVALIDSET
    {
      get { return _noValidYears; }
    }

    private HashSet<int> _validYears;
    private int _minimumYears = 0;
    private int _maximumYears = 0;

    public static TldValidYearsSet FromPeriodElements(IEnumerable<XElement> periodElements)
    {
      return new TldValidYearsSet(periodElements);
    }

    public static TldValidYearsSet FromNothing()
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
      get
      {
        return _minimumYears;
      }
    }

    public int Max
    {
      get
      {
        return _maximumYears;
      }
    }
    
  }
}
