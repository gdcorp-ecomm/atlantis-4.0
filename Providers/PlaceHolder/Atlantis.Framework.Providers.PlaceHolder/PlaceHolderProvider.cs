using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  public class PlaceHolderProvider : ProviderBase, IPlaceHolderProvider
  {
    private const string PLACE_HOLDER_TYPE = "placeholdertype";
    private const string PLACE_HOLDER_DATA = "placeholderdata";

    private static readonly Regex _placeHolderRegex = new Regex(@"\[@P\[(?<placeholdertype>[a-zA-z0-9]*?):(?<placeholderdata>.*?)\]@P\]", RegexOptions.Compiled | RegexOptions.Singleline);
    
    private readonly ICollection<string> _debugContextErrors = new Collection<string>(); 

    public PlaceHolderProvider(IProviderContainer container) : base(container)
    {
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
        IList<KeyValuePair<string, IPlaceHolderHandler>> placeHolderHandlers = GetPlaceHolderHandlers(placeHolderMatches);
        finalContent = ProcessPlaceHolderHandlers(placeHolderHandlers, originalContent);

        LogDebugContextData();
      }

      return finalContent;
    }

    private IList<KeyValuePair<string, IPlaceHolderHandler>> GetPlaceHolderHandlers(MatchCollection placeHolderMatches)
    {
      IList<KeyValuePair<string, IPlaceHolderHandler>> placeHolderHandlers = new List<KeyValuePair<string, IPlaceHolderHandler>>(placeHolderMatches.Count);
      IPlaceHolderHandlerFactory placeHolderHandlerFactory = new PlaceHolderHandlerHandlerFactory();

      foreach (Match placeHolderMatch in placeHolderMatches)
      {
        string matchValue = placeHolderMatch.Value;
        string placeHolderType = placeHolderMatch.Groups[PLACE_HOLDER_TYPE].Captures[0].Value;
        string placeHolderDataRaw = placeHolderMatch.Groups[PLACE_HOLDER_DATA].Captures[0].Value;

        IPlaceHolderHandler placeHolderHandler = placeHolderHandlerFactory.ConstructHandler(placeHolderType, placeHolderDataRaw, _debugContextErrors, Container);

        placeHolderHandlers.Add(new KeyValuePair<string, IPlaceHolderHandler>(matchValue, placeHolderHandler));
      }

      return placeHolderHandlers;
    }

    private string ProcessPlaceHolderHandlers(IList<KeyValuePair<string, IPlaceHolderHandler>> placeHolderHandlers, string originalContent)
    {
      RaisePlaceHolderEvents(placeHolderHandlers);
      return RenderPlaceHolders(placeHolderHandlers, originalContent);
    }

    private void RaisePlaceHolderEvents(IList<KeyValuePair<string, IPlaceHolderHandler>> placeHolderHandlers)
    {
      foreach (KeyValuePair<string, IPlaceHolderHandler> placeHolderHandler in placeHolderHandlers)
      {
        placeHolderHandler.Value.RaiseInitEvent();
      }

      foreach (KeyValuePair<string, IPlaceHolderHandler> placeHolderHandler in placeHolderHandlers)
      {
        placeHolderHandler.Value.RaiseLoadEvent();
      }

      foreach (KeyValuePair<string, IPlaceHolderHandler> placeHolderHandler in placeHolderHandlers)
      {
        placeHolderHandler.Value.RaisePreRenderEvent();
      }
    }

    private string RenderPlaceHolders(IList<KeyValuePair<string, IPlaceHolderHandler>> placeHolderHandlers, string originalContent)
    {
      StringBuilder contentBuilder = new StringBuilder(originalContent);

      foreach (KeyValuePair<string, IPlaceHolderHandler> placeHolderHandler in placeHolderHandlers)
      {
        contentBuilder.Replace(placeHolderHandler.Key, placeHolderHandler.Value.Render());
      }

      return contentBuilder.ToString();
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