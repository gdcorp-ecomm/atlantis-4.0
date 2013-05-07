using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  public class PlaceHolderProvider : ProviderBase, IPlaceHolderProvider
  {
    private const string PLACE_HOLDER_KEY = "placeholderkey";
    private const string PLACE_HOLDER_DATA = "placeholderdata";

    private static readonly Regex _placeHolderRegex = new Regex(@"\[@P\[(?<placeholderkey>[a-zA-z0-9]*?):(?<placeholderdata>.*?)\]@P\]", RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly IDictionary<string, IPlaceHolderHandler> _placeHolderHandlers = new Dictionary<string, IPlaceHolderHandler>(StringComparer.OrdinalIgnoreCase);

    static PlaceHolderProvider()
    {
      IPlaceHolderHandler userControlPlaceHolderHandler = new UserControlPlaceHolderHandler();

      _placeHolderHandlers[userControlPlaceHolderHandler.Name] = userControlPlaceHolderHandler;
    }

    public PlaceHolderProvider(IProviderContainer container) : base(container)
    {
    }

    public string ReplacePlaceHolders(string content)
    {
      string originalContent = content ?? string.Empty;

      string finalContent = originalContent;

      if (originalContent != string.Empty)
      {
        MatchCollection placeHolderMatches = _placeHolderRegex.Matches(originalContent);

        if (placeHolderMatches.Count > 0)
        {
          StringBuilder contentBuilder = new StringBuilder(originalContent);

          foreach (Match placeHolderMatch in placeHolderMatches)
          {
            string matchValue = placeHolderMatch.Value;
            string placeHolderContent = ProcessPlaceHolderContent(placeHolderMatch);

            contentBuilder.Replace(matchValue, placeHolderContent);
          }

          finalContent = contentBuilder.ToString();
        }
      }

      return finalContent;
    }

    private string ProcessPlaceHolderContent(Match placeHolderMatch)
    {
      string placeHolderKey = placeHolderMatch.Groups[PLACE_HOLDER_KEY].Captures[0].Value;
      string placeHolderData = placeHolderMatch.Groups[PLACE_HOLDER_DATA].Captures[0].Value;

      IPlaceHolderHandler placeHolderHandler = DeterminePlaceHolderHandler(placeHolderKey);

      return placeHolderHandler.GetPlaceHolderContent(placeHolderKey, placeHolderData, Container);
    }

    private IPlaceHolderHandler DeterminePlaceHolderHandler(string placeHolderKey)
    {
      IPlaceHolderHandler placeHolderHandler;
      
      if (!_placeHolderHandlers.TryGetValue(placeHolderKey, out placeHolderHandler))
      {
        placeHolderHandler = new NullPlaceHolderHandler();
      }

      return placeHolderHandler;
    }
  }
}
