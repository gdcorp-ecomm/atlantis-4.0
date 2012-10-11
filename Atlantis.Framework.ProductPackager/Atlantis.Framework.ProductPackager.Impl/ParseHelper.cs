using System;

namespace Atlantis.Framework.ProductPackager.Impl
{
  internal static class ParseHelper
  {
    internal static int ParseInt(string value, string errorParseFormat)
    {
      int parsedValue;

      if (!int.TryParse(value, out parsedValue))
      {
        throw new Exception(string.Format(errorParseFormat, value));
      }

      return parsedValue;
    }

    internal static bool ParseBool(string value, string errorParseFormat)
    {
      bool parsedValue;

      if (!bool.TryParse(value, out parsedValue))
      {
        throw new Exception(string.Format(errorParseFormat, value));
      }

      return parsedValue;
    }

    internal static double ParseDouble(string value, string errorParseFormat)
    {
      double parsedValue;

      if (!double.TryParse(value, out parsedValue))
      {
        throw new Exception(string.Format(errorParseFormat, value));
      }

      return parsedValue;
    }
  }
}
