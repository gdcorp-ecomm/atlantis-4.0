using System;
using System.Xml.Linq;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  internal static class XElementExtensions
  {
    public static bool IsEnabled(this XElement element, bool defaultIfMissing = false)
    {
      bool result = defaultIfMissing;

      XAttribute enabled = element.Attribute("enabled");
      if (enabled != null)
      {
        result = "true".Equals(enabled.Value, StringComparison.OrdinalIgnoreCase);
      }

      return result;
    }

    public static double PeriodYears(this XElement element, double defaultIfMissing = 0)
    {
      double result = defaultIfMissing;

      XAttribute unitAttribute = element.Attribute("unit");
      XAttribute valueAttribute = element.Attribute("value");

      if ((unitAttribute != null) && (valueAttribute != null))
      {
        int valueInt;
        if (int.TryParse(valueAttribute.Value, out valueInt))
        {
          TldPeriodUnit foundUnit = ParsePeriodUnit(unitAttribute.Value);
          if (foundUnit == TldPeriodUnit.Years)
          {
            result = (double)valueInt;
          }
          else if (foundUnit == TldPeriodUnit.Months)
          {
            result = valueInt / 12d;
          }
        }
      }

      return result;
    }

    public static int PeriodMonths(this XElement element, int defaultIfMissing = 0)
    {
      int result = defaultIfMissing;

      XAttribute unitAttribute = element.Attribute("unit");
      XAttribute valueAttribute = element.Attribute("value");

      if ((unitAttribute != null) && (valueAttribute != null))
      {
        int valueInt;
        if (int.TryParse(valueAttribute.Value, out valueInt))
        {
          TldPeriodUnit foundUnit = ParsePeriodUnit(unitAttribute.Value);
          if (foundUnit == TldPeriodUnit.Years)
          {
            result = valueInt * 12;
          }
          else if (foundUnit == TldPeriodUnit.Months)
          {
            result = valueInt;
          }
        }
      }

      return result;
    }

    public static int PeriodDays(this XElement element, int defaultIfMissing = 0)
    {
      int result = defaultIfMissing;

      XAttribute unitAttribute = element.Attribute("unit");
      XAttribute valueAttribute = element.Attribute("value");

      if ((unitAttribute != null) && (valueAttribute != null))
      {
        int valueInt;
        if (int.TryParse(valueAttribute.Value, out valueInt))
        {
          TldPeriodUnit foundUnit = ParsePeriodUnit(unitAttribute.Value);

          if (foundUnit == TldPeriodUnit.Days)
          {
            result = valueInt;
          }
        }
      }

      return result;
    }

    private static TldPeriodUnit ParsePeriodUnit(string unit)
    {
      TldPeriodUnit result = TldPeriodUnit.Unknown;

      switch (unit.ToLowerInvariant())
      {
        case "year":
          result = TldPeriodUnit.Years;
          break;
        case "month":
          result = TldPeriodUnit.Months;
          break;
        case "day":
          result = TldPeriodUnit.Days;
          break;
      }

      return result;
    }
  }
}
