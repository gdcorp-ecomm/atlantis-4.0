using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Render.Pipeline;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Tokens.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Atlantis.Framework.Providers.CDSContent
{
  public class CDSContentProvider : ProviderBase, ICDSContentProvider
  {
    const string WhiteListFormat = "content/{0}/whitelist";
    const string RulesDocFormat = "content/{0}/{1}.rule";
    const string DefaultContentPathFormat = "content/{0}/{1}";
    const string ContentPathFormat = "content/{0}";

    private static readonly IRedirectResult NullRedirectResult = new RedirectResult(false, null);

    private IProviderContainer _container;
    private readonly ISiteContext _siteContext;
    private readonly IShopperContext _shopperContext;
    private ExpressionParserManager _expressionParser;
    
    public CDSContentProvider(IProviderContainer container)
      : base(container)
    {
      _container = container;
      _siteContext = container.Resolve<ISiteContext>();
      _shopperContext = container.Resolve<IShopperContext>();
      _expressionParser = new ExpressionParserManager(_container);
      _expressionParser.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;
    }

    #region ICDSContentProvider Members

    public IWhitelistResult CheckWhiteList(string appName, string relativePath)
    {
      IWhitelistResult result = UrlWhitelistResponseData.NullWhielistResult;

      if (!string.IsNullOrEmpty(appName) && !string.IsNullOrEmpty(relativePath))
      {
        ProcessQuery cdsQuery = new ProcessQuery(_container, string.Format(WhiteListFormat, appName));
        var requestData = new CDSRequestData(_shopperContext.ShopperId, string.Empty, string.Empty, _siteContext.Pathway, _siteContext.PageCount, cdsQuery.Query);
        try
        {
          UrlWhitelistResponseData responseData = cdsQuery.BypassCache ? (UrlWhitelistResponseData)Engine.Engine.ProcessRequest(requestData, CDSProviderEngineRequests.UrlWhitelistRequestType) : (UrlWhitelistResponseData)DataCache.DataCache.GetProcessRequest(requestData, CDSProviderEngineRequests.UrlWhitelistRequestType);
          if (responseData != null && responseData.IsSuccess)
          {
            result = responseData.CheckWhitelist(relativePath);
          }
        }
        catch (Exception ex)
        {
          Engine.Engine.LogAtlantisException(new AtlantisException(ex.Source, string.Empty, "0", ex.Message, cdsQuery.Query, string.Empty, string.Empty, string.Empty, string.Empty, 0));
        }
      }

      return result;
    }

    public IRedirectResult CheckRedirectRules(string appName, string relativePath)
    {
      IRedirectResult redirectResult = NullRedirectResult;
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
              TokenManager.ReplaceTokens(rawRedirectData.Location, _container, out resultText);
              redirectResult = new RedirectResult(true, new RedirectData(rawRedirectData.Type, resultText));
            }
          }
        }
        catch (Exception ex)
        {
          Engine.Engine.LogAtlantisException(new AtlantisException(ex.Source, string.Empty, "0", ex.Message, appName + "-" + relativePath, string.Empty, string.Empty, string.Empty, string.Empty, 0));
        }
      }

      return redirectResult;
    }

    public string GetContent(string appName, string relativePath, RenderPipelineManager renderPipelineManager)
    {
      string processedText = string.Empty;

      string contentPath = GetContentPath(appName, relativePath);
      if (!string.IsNullOrEmpty(contentPath))
      {
        ProcessQuery cdsQuery = new ProcessQuery(_container, contentPath);
        var requestData = new CDSRequestData(_shopperContext.ShopperId, string.Empty, string.Empty, _siteContext.Pathway, _siteContext.PageCount, cdsQuery.Query);
        try
        {
          ContentVersionResponseData responseData = cdsQuery.BypassCache ? (ContentVersionResponseData)Engine.Engine.ProcessRequest(requestData, CDSProviderEngineRequests.ContentVersionRequestType) : (ContentVersionResponseData)DataCache.DataCache.GetProcessRequest(requestData, CDSProviderEngineRequests.ContentVersionRequestType);
          if (responseData != null && responseData.IsSuccess && !string.IsNullOrEmpty(responseData.Content))
          {
            processedText = renderPipelineManager.RenderContent(responseData, _container).Content;
          }
        }
        catch (Exception ex)
        {
          Engine.Engine.LogAtlantisException(new AtlantisException(ex.Source, string.Empty, "0", ex.Message, cdsQuery.Query, string.Empty, string.Empty, string.Empty, string.Empty, 0));
        }
      }

      return processedText;
    }
    
    #endregion

    #region Private

    private ReadOnlyCollection<IRoutingRule> GetRoutingRules(string appName, string relativePath, string type)
    {
      ReadOnlyCollection<IRoutingRule> routingRules = null;

      if (!string.IsNullOrEmpty(appName) && !string.IsNullOrEmpty(relativePath))
      {
        ProcessQuery cdsQuery = new ProcessQuery(_container, string.Format(RulesDocFormat, appName, relativePath));
        var requestData = new CDSRequestData(_shopperContext.ShopperId, string.Empty, string.Empty, _siteContext.Pathway, _siteContext.PageCount, cdsQuery.Query);
        try
        {
          RoutingRulesResponseData responseData = cdsQuery.BypassCache ? (RoutingRulesResponseData)Engine.Engine.ProcessRequest(requestData, CDSProviderEngineRequests.RoutingRulesRequestType) : (RoutingRulesResponseData)DataCache.DataCache.GetProcessRequest(requestData, CDSProviderEngineRequests.RoutingRulesRequestType);
          if (responseData != null && responseData.IsSuccess)
          {
            responseData.TryGetValue(type, out routingRules);
          }
        }
        catch (Exception ex)
        {
          Engine.Engine.LogAtlantisException(new AtlantisException(ex.Source, string.Empty, "0", ex.Message, cdsQuery.Query, string.Empty, string.Empty, string.Empty, string.Empty, 0));
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
              result = _expressionParser.EvaluateExpression(rule.Condition);
            }
            catch (Exception ex)
            {
              Engine.Engine.LogAtlantisException(new AtlantisException("Error evaluating condition: " + ex.Source, string.Empty, "0", ex.Message, "Condition: " + rule.Condition, string.Empty, string.Empty, string.Empty, string.Empty, 0));
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
              Engine.Engine.LogAtlantisException(new AtlantisException("Error deserializing rule data: " + ex.Source, string.Empty, "0", ex.Message, appName + "-" + relativePath + " - RuleDataString: " + ruleData, string.Empty, string.Empty, string.Empty, string.Empty, 0));
            }
          }
        }
      }
      return contentPath;
    }

    #endregion
  }
}
