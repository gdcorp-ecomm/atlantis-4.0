using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    private readonly IDictionary<string, IPlaceHolderData> _placeHolderSharedData = new Dictionary<string, IPlaceHolderData>(256);
    private readonly ICollection<string> _debugContextErrors = new Collection<string>(); 

    static PlaceHolderProvider()
    {
      InitializePlaceHolders();
    }

    private static void InitializePlaceHolders()
    {
      IPlaceHolderHandler userControlPlaceHolderHandler = new UserControlPlaceHolderHandler();

      _placeHolderHandlers[userControlPlaceHolderHandler.Type] = userControlPlaceHolderHandler;
    }

    public PlaceHolderProvider(IProviderContainer container) : base(container)
    {
    }

    public IPlaceHolderData GetPlaceHolderData(string type)
    {
      IPlaceHolderData placeHolderData;
      
      if (!_placeHolderSharedData.TryGetValue(type, out placeHolderData))
      {
        placeHolderData = new XmlPlaceHolderData();
      }

      return placeHolderData;
    }

    public string GetPlaceHolderMarkup(string type, string location, IDictionary<string, string> parameters)
    {
      PlaceHolder placeHolder = new PlaceHolder(type, location, parameters);
      return placeHolder.ToMarkup();
    }

    public string ReplacePlaceHolders(string content)
    {
      return ReplacePlaceHolders(content, null);
    }

    [Obsolete("Only used in the legacy CDS widget context, use ReplacePlaceHolder(string content) instead.")]
    public string ReplacePlaceHolders(string content, IPlaceHolderEncoding placeHolderEncoding)
    {
      string originalContent = content ?? string.Empty;
      string finalContent = string.Empty;

      if (originalContent != string.Empty)
      {
        finalContent = ProcessPlaceHolderMatches(originalContent, placeHolderEncoding);
      }

      return finalContent;
    }

    private string ProcessPlaceHolderMatches(string originalContent, IPlaceHolderEncoding placeHolderEncoding)
    {
      string finalContent = originalContent;

      MatchCollection placeHolderMatches = _placeHolderRegex.Matches(originalContent);

      if (placeHolderMatches.Count > 0)
      {
        StringBuilder contentBuilder = new StringBuilder(originalContent);

        foreach (Match placeHolderMatch in placeHolderMatches)
        {
          ProcessPlaceHolderMatch(placeHolderMatch, contentBuilder, placeHolderEncoding);
        }

        finalContent = contentBuilder.ToString();

        LogDebugContextData();
      }

      return finalContent;
    }

    private void ProcessPlaceHolderMatch(Match placeHolderMatch, StringBuilder contentBuilder, IPlaceHolderEncoding placeHolderEncoding)
    {
      string matchValue = placeHolderMatch.Value;
      string placeHolderKey = placeHolderMatch.Groups[PLACE_HOLDER_KEY].Captures[0].Value;
      string placeHolderDataString = placeHolderMatch.Groups[PLACE_HOLDER_DATA].Captures[0].Value;

      if (placeHolderEncoding != null)
      {
        placeHolderDataString = placeHolderEncoding.DecodePlaceHolderData(placeHolderDataString);
      }

      IPlaceHolderHandler placeHolderHandler = DeterminePlaceHolderHandler(placeHolderKey);

      string content = placeHolderHandler.GetPlaceHolderContent(placeHolderKey, placeHolderDataString, _placeHolderSharedData, _debugContextErrors, Container);

      if (placeHolderEncoding != null)
      {
        content = placeHolderEncoding.EncodePlaceHolderResult(content);
      }

      contentBuilder.Replace(matchValue, content);
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

    private void LogDebugContextData()
    {
      IDebugContext debugContext;
      if (_debugContextErrors.Count > 0 && Container.TryResolve(out debugContext))
      {
        StringBuilder placeHolderDebugBuilder = new StringBuilder();
        foreach (string debugContextError in _debugContextErrors)
        {
          placeHolderDebugBuilder.AppendLine(debugContextError);
        }

        debugContext.LogDebugTrackingData("PlaceHolder Errors", placeHolderDebugBuilder.ToString());
      }
    }
  }
}
