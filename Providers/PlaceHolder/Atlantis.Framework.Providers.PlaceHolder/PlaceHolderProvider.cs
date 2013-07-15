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
    private const string PLACE_HOLDER_TYPE = "placeholdertype";
    private const string PLACE_HOLDER_DATA = "placeholderdata";

    private static readonly Regex _placeHolderRegex = new Regex(@"\[@P\[(?<placeholdertype>[a-zA-z0-9]*?):(?<placeholderdata>.*?)\]@P\]", RegexOptions.Compiled | RegexOptions.Singleline);
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
      IPlaceHolderHandler cdsDocumentPlaceHolderHandler = new CDSDocumentPlaceHolderHandler();
      IPlaceHolderHandler webControlPlaceHolderHandler = new WebControlPaceHolderHandler();

      _placeHolderHandlers[userControlPlaceHolderHandler.Type] = userControlPlaceHolderHandler;
      _placeHolderHandlers[cdsDocumentPlaceHolderHandler.Type] = cdsDocumentPlaceHolderHandler;
      _placeHolderHandlers[webControlPlaceHolderHandler.Type] = webControlPlaceHolderHandler;
    }

    public PlaceHolderProvider(IProviderContainer container) : base(container)
    {
    }

    public IPlaceHolderData GetPlaceHolderData(string id)
    {
      IPlaceHolderData placeHolderData;
      
      if (!_placeHolderSharedData.TryGetValue(id, out placeHolderData))
      {
        placeHolderData = new PlaceHolderData();
      }

      return placeHolderData;
    }

    public string ReplacePlaceHolders(string content)
    {
      string originalContent = content ?? string.Empty;
      string finalContent = string.Empty;

      if (originalContent != string.Empty)
      {
        finalContent = ProcessPlaceHolderMatches(originalContent);
      }

      return finalContent;
    }

    private string ProcessPlaceHolderMatches(string originalContent)
    {
      string finalContent = originalContent;

      MatchCollection placeHolderMatches = _placeHolderRegex.Matches(originalContent);

      if (placeHolderMatches.Count > 0)
      {
        StringBuilder contentBuilder = new StringBuilder(originalContent);

        foreach (Match placeHolderMatch in placeHolderMatches)
        {
          ProcessPlaceHolderMatch(placeHolderMatch, contentBuilder);
        }

        finalContent = contentBuilder.ToString();

        LogDebugContextData();
      }

      return finalContent;
    }

    private void ProcessPlaceHolderMatch(Match placeHolderMatch, StringBuilder contentBuilder)
    {
      string matchValue = placeHolderMatch.Value;
      string placeHolderType = placeHolderMatch.Groups[PLACE_HOLDER_TYPE].Captures[0].Value;
      string placeHolderDataString = placeHolderMatch.Groups[PLACE_HOLDER_DATA].Captures[0].Value;

      IPlaceHolderHandler placeHolderHandler = DeterminePlaceHolderHandler(placeHolderType);

      string content = placeHolderHandler.GetPlaceHolderContent(placeHolderType, placeHolderDataString, _placeHolderSharedData, _debugContextErrors, Container);

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