using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Tokens.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atlantis.Framework.Providers.CDSContent
{
  public class CDSContentProvider : ProviderBase, ICDSContentProvider
  {
    private const string WhiteListFormat = "content/{0}/whitelist";
    private const string RulesDocFormat = "content/{0}/{1}.rule";
    private const string DefaultContentPathFormat = "content/{0}/{1}";
    private const string ContentPathFormat = "content/{0}";

    private static readonly IRedirectResult _nullRedirectResult = new RedirectResult(false, null);
    private static readonly IRenderContent _nullRenderContent = new ContentVersionResponseData(null);

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
      IWhitelistResult whitelistResult;

      string cdsPath = string.Format(WhiteListFormat, appName);

      CDSRequestData requestData = new CDSRequestData(cdsPath);
        
      try
      {
        UrlWhitelistResponseData responseData = (UrlWhitelistResponseData)DataCache.DataCache.GetProcessRequest(requestData, CDSProviderEngineRequests.UrlWhitelistRequestType);
        whitelistResult = responseData.CheckWhitelist(relativePath);
      }
      catch (Exception ex)
      {
        whitelistResult = UrlWhitelistResponseData.NullWhitelistResult;

        Engine.Engine.LogAtlantisException(new AtlantisException("CDSContentProvider.CheckWhiteList()", 
                                                                 "0", 
                                                                 "CDSContentProvider whitelist error. " + ex.Message, 
                                                                 cdsPath,
                                                                 null,
                                                                 null));
      }

      return whitelistResult;
    }

    public IRedirectResult CheckRedirectRules(string appName, string relativePath)
    {
      IRedirectResult redirectResult = _nullRedirectResult;

      ReadOnlyCollection<IRoutingRule> redirectRules = GetRoutingRules(appName, relativePath, RoutingRuleTypes.Redirect);

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
              TokenManager.ReplaceTokens(rawRedirectData.Location, Container, out resultText);

              redirectResult = new RedirectResult(true, new RedirectData(rawRedirectData.Type, resultText));
            }
          }
        }
        catch (Exception ex)
        {
          Engine.Engine.LogAtlantisException(new AtlantisException("CDSContentProvider.CheckRedirectRules()", 
                                                                   "0", 
                                                                   "CDSContentProvider redirect rule error. " + ex.Message,
                                                                   appName + relativePath,
                                                                   null,
                                                                   null));
        }
      }

      return redirectResult;
    }

    public IRenderContent GetContent(string appName, string relativePath)
    {
      IRenderContent contentVersion = _nullRenderContent;

      string contentPath = GetContentPath(appName, relativePath);

      if (!string.IsNullOrEmpty(contentPath))
      {
        ProcessQuery cdsQuery = new ProcessQuery(Container, contentPath);
        var requestData = new CDSRequestData(cdsQuery.Query);

        try
        {
          ContentVersionResponseData responseData = cdsQuery.BypassCache ? (ContentVersionResponseData)Engine.Engine.ProcessRequest(requestData, CDSProviderEngineRequests.ContentVersionRequestType) : (ContentVersionResponseData)DataCache.DataCache.GetProcessRequest(requestData, CDSProviderEngineRequests.ContentVersionRequestType);
          
          if (responseData.IsSuccess && !string.IsNullOrEmpty(responseData.Content))
          {
            contentVersion = responseData;
          }
        }
        catch (Exception ex)
        {
          Engine.Engine.LogAtlantisException(new AtlantisException("CDSContentProvider.GetContent()", 
                                                                   "0", 
                                                                   "CDSContentProvider error getting content. " + ex.Message,
                                                                   contentPath,
                                                                   null,
                                                                   null));
        }
      }

      return contentVersion;
    }
    
    private ReadOnlyCollection<IRoutingRule> GetRoutingRules(string appName, string relativePath, string type)
    {
      ReadOnlyCollection<IRoutingRule> routingRules = null;

      if (!string.IsNullOrEmpty(appName) && !string.IsNullOrEmpty(relativePath))
      {
        string cdsPath = string.Format(RulesDocFormat, appName, relativePath);
        ProcessQuery cdsQuery = new ProcessQuery(Container, null);

        var requestData = new CDSRequestData(cdsPath);

        try
        {
          RoutingRulesResponseData responseData = cdsQuery.BypassCache ? (RoutingRulesResponseData)Engine.Engine.ProcessRequest(requestData, CDSProviderEngineRequests.RoutingRulesRequestType) : (RoutingRulesResponseData)DataCache.DataCache.GetProcessRequest(requestData, CDSProviderEngineRequests.RoutingRulesRequestType);
          
          if (responseData.IsSuccess)
          {
            responseData.TryGetValue(type, out routingRules);
          }
        }
        catch (Exception ex)
        {
          Engine.Engine.LogAtlantisException(new AtlantisException("CDSContentProvider.GetRoutingRules()", 
                                                                   "0", 
                                                                   "CDSContentProvider error getting route rules. " + ex.Message,
                                                                   cdsPath,
                                                                   null,
                                                                   null));
        }
      }

      return routingRules;
    }

    private string GetRuleData(IEnumerable<IRoutingRule> rules)
    {
      string ruleData = null;

      if (rules != null)
      {
        foreach (IRoutingRule rule in rules)
        {
          bool result;
          if (!string.IsNullOrEmpty(rule.Condition))
          {
            try
            {
              result = ExpressionParserManager.EvaluateExpression(rule.Condition);
            }
            catch (Exception ex)
            {
              Engine.Engine.LogAtlantisException(new AtlantisException("CDSContentProvider.GetRuleData()", 
                                                                       "0", 
                                                                       "CDSContentProvider error evaluating rule condition. " + ex.Message,
                                                                       "Condition: " + rule.Condition,
                                                                       null,
                                                                       null));
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

        ReadOnlyCollection<IRoutingRule> routeRules = GetRoutingRules(appName, relativePath, RoutingRuleTypes.Route);
        
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
              Engine.Engine.LogAtlantisException(new AtlantisException("CDSContentProvider.GetContentPath()", 
                                                                       "0", 
                                                                       "CDSContentProvider error deserializing rule data. " + ex.Message,
                                                                       contentPath,
                                                                       null,
                                                                       null));
            }
          }
        }
      }

      return contentPath;
    }
  }
}