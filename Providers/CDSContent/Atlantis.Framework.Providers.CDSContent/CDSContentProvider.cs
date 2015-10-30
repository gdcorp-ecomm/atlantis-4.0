using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Render.Containers;
using Atlantis.Framework.Tokens.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atlantis.Framework.Providers.CDSContent
{
  public class CDSContentProvider : ProviderBase, ICDSContentProvider
  {
    private const string DefaultContentPathFormat = "content/{0}/{1}";
    private const string ContentPathFormat = "content/{0}";

    private static readonly IRedirectResult _nullRedirectResult = new RedirectResult(false, null);

    private TokenProvider TokenProvider
    {
      get
      {
        return new TokenProvider(Container);
      }
    }


    private ExpressionParserManager _expressionParserManager;
    private ExpressionParserManager ExpressionParserManager
    {
      get
      {
        if (_expressionParserManager == null)
        {
          _expressionParserManager = new ExpressionParserManager(Container);
          _expressionParserManager.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;
        }
        return _expressionParserManager;
      }
    }
    
    public CDSContentProvider(IProviderContainer container) : base(container)
    {
    }

    public IWhitelistResult CheckWhiteList(string appName, string relativePath)
    {
      WhitelistDocument whitelist = new WhitelistDocument(Container, appName);
      return whitelist.CheckWhiteList(relativePath);
    }

    public IRedirectResult CheckRedirectRules(string appName, string relativePath)
    {
      IRedirectResult redirectResult = _nullRedirectResult;

      RulesDocument rulesDoc = new RulesDocument(Container, appName, relativePath);

      ReadOnlyCollection<IRoutingRule> redirectRules = rulesDoc.GetRoutingRules(RoutingRuleTypes.Redirect);

      if (redirectRules != null)
      {
        try
        {
          string ruleData = GetRuleData(redirectRules);

          if (!string.IsNullOrEmpty(ruleData))
          {
            RedirectData rawRedirectData = JsonConvert.DeserializeObject<RedirectData>(ruleData);

            if (!string.IsNullOrEmpty(rawRedirectData.Location))
            {
              string resultText;
              TokenProvider.ReplaceTokens(ProviderContainerDataTokenManager.ReplaceDataTokens(rawRedirectData.Location, Container), out resultText);

              redirectResult = new RedirectResult(true, new RedirectData(rawRedirectData.Type, resultText));
            }
          }
        }
        catch (Exception ex)
        {
          Engine.Engine.LogAtlantisException(new AtlantisException("CDSContentProvider.CheckRedirectRules()", 0, "CDSContentProvider redirect rule error. " + ex.Message, appName + relativePath));
        }
      }

      return redirectResult;
    }

    public IRenderContent GetContent(string appName, string relativePath)
    {
      IRenderContent contentVersion = ContentDocument.NullRenderContent;
      
      string contentPath = GetContentPath(appName, relativePath);

      if (!string.IsNullOrEmpty(contentPath))
      {
        ContentDocument contentDoc = new ContentDocument(Container, contentPath);
        contentVersion = contentDoc.GetContent();        
      }

      return contentVersion;
    }
   
    private string GetRuleData(IEnumerable<IRoutingRule> rules)
    {
      string ruleData = null;

      if (rules != null)
      {
        foreach (IRoutingRule rule in rules)
        {
          bool result;
          string resultText = ProviderContainerDataTokenManager.ReplaceDataTokens(rule.Condition, Container);
          if (!string.IsNullOrEmpty(resultText))
          {
            try
            {
              result = ExpressionParserManager.EvaluateExpression(resultText);
            }
            catch (Exception ex)
            {
              Engine.Engine.LogAtlantisException(new AtlantisException("CDSContentProvider.GetRuleData()", 0, "CDSContentProvider error evaluating rule condition. " + ex.Message, "Condition: " + resultText));
              result = false;
            }
            if (result && !string.IsNullOrEmpty(rule.Data))
            {
              ruleData = rule.Data;
              break;
            }
          }
        }
      }

      return ruleData;
    }

    internal string GetContentPath(string appName, string relativePath)
    {
      string contentPath = string.Empty;

      if (!string.IsNullOrEmpty(appName) && !string.IsNullOrEmpty(relativePath))
      {
        contentPath = string.Format(DefaultContentPathFormat, appName, relativePath);

        RulesDocument rulesDoc = new RulesDocument(Container, appName, relativePath);

        ReadOnlyCollection<IRoutingRule> routeRules = rulesDoc.GetRoutingRules(RoutingRuleTypes.Route);
        
        if (routeRules != null)
        {
          string ruleData = GetRuleData(routeRules);
          
          if (!string.IsNullOrEmpty(ruleData))
          {
            try
            {
              RouteData rawRouteData = JsonConvert.DeserializeObject<RouteData>(ruleData);
              
              if (!string.IsNullOrEmpty(rawRouteData.Location))
              {
                contentPath = string.Format(ContentPathFormat, rawRouteData.Location);
              }
            }
            catch (Exception ex)
            {
              Engine.Engine.LogAtlantisException(new AtlantisException("CDSContentProvider.GetContentPath()", 0, "CDSContentProvider error deserializing rule data. " + ex.Message, contentPath));
            }
          }
        }
      }

      return contentPath;
    }
     
  }
}