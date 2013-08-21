using System.Collections.Generic;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal static class PlaceHolderHandlerManager
  {
    private const string PLACE_HOLDER_TYPE = "placeholdertype";
    private const string PLACE_HOLDER_DATA = "placeholderdata";

    private static readonly Regex _placeHolderRegex = new Regex(@"\[@P\[(?<placeholdertype>[a-zA-z0-9]*?):(?<placeholderdata>.*?)\]@P\]", RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly IList<IPlaceHolderHandler> _emptyPlaceHolderHandlersList = new List<IPlaceHolderHandler>(0);

    internal static IList<IPlaceHolderHandler> GetPlaceHolderHandlers(string content, ICollection<string> debugContextErrors, IProviderContainer container)
    {
      IList<IPlaceHolderHandler> placeHolderHandlers;

      MatchCollection placeHolderMatches = _placeHolderRegex.Matches(content);

      if (placeHolderMatches.Count == 0)
      {
        placeHolderHandlers = _emptyPlaceHolderHandlersList;
      }
      else
      {
        placeHolderHandlers = new List<IPlaceHolderHandler>(placeHolderMatches.Count);
        IPlaceHolderHandlerFactory placeHolderHandlerFactory = new PlaceHolderHandlerHandlerFactory();

        foreach (Match placeHolderMatch in placeHolderMatches)
        {
          string matchValue = placeHolderMatch.Value;
          string placeHolderType = placeHolderMatch.Groups[PLACE_HOLDER_TYPE].Captures[0].Value;
          string placeHolderDataRaw = placeHolderMatch.Groups[PLACE_HOLDER_DATA].Captures[0].Value;

          IPlaceHolderHandler placeHolderHandler = placeHolderHandlerFactory.ConstructHandler(placeHolderType, matchValue, placeHolderDataRaw, debugContextErrors, container);

          placeHolderHandlers.Add(placeHolderHandler);
        }
      }

      return placeHolderHandlers;
    }
  }
}
