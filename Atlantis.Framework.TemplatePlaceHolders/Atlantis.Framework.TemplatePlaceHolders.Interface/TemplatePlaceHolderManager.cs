using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  public class TemplatePlaceHolderManager
  {
    private const string TEMPLATE_DATA_KEY = "templatedata";
    private static readonly Regex _templatePlaceHolderRegex = new Regex(@"\[@TemplatePlaceHolder\[(?<templatedata>.*?)\]@TemplatePlaceHolder\]", RegexOptions.Compiled | RegexOptions.Singleline);

    private static IList<ITemplatePlaceHolder> InitializeTemplatePlaceHolders(IList<Match> placeHoldersMatches)
    {
      IList<ITemplatePlaceHolder> templatePlaceHolders = new List<ITemplatePlaceHolder>(placeHoldersMatches.Count);

      foreach (Match placeHoldersMatch in placeHoldersMatches)
      {
        string templateDataString = placeHoldersMatch.Groups[TEMPLATE_DATA_KEY].Captures[0].Value;
        if (!string.IsNullOrEmpty(templateDataString))
        {
          ITemplatePlaceHolder templatePlaceHolder = TemplatePlaceHolder.GetInstance(placeHoldersMatch.Value, templateDataString);
          if(templatePlaceHolder != null)
          {
            templatePlaceHolders.Add(templatePlaceHolder);
          }
        }
      }

      return templatePlaceHolders;
    }

    private static IList<Match> ParsePlaceHolderStrings(string inputText)
    {
      IList<Match> result = new List<Match>();

      MatchCollection matches = _templatePlaceHolderRegex.Matches(inputText);
      foreach (Match match in matches)
      {
        result.Add(match);
      }

      return result;
    }

    public static string ReplacePlaceHolders(string inputText, IProviderContainer providerContainer)
    {
      string resultText;

      IList<Match> foundPlaceHolders = ParsePlaceHolderStrings(inputText);
      if(foundPlaceHolders.Count == 0)
      {
        resultText = inputText;
      }
      else
      {
        StringBuilder workingText = new StringBuilder(inputText);
        IList<ITemplatePlaceHolder> templatePlaceHolders = InitializeTemplatePlaceHolders(foundPlaceHolders);

        ITemplatePlaceHolderEvaluator templatePlaceHolderEvaluator = new TemplatePlaceHolderEvaluator();
        foreach (ITemplatePlaceHolder templatePlaceHolder in templatePlaceHolders)
        {
          string replacementContent = templatePlaceHolderEvaluator.EvaluatePlaceHolder(templatePlaceHolder, providerContainer);
          workingText.Replace(templatePlaceHolder.PlaceHolder, replacementContent);
        }

        resultText = workingText.ToString();
      }

      return resultText;
    }
  }
}
