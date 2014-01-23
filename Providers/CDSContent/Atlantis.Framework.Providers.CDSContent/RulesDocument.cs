using System;
using System.Collections.ObjectModel;

using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;

namespace Atlantis.Framework.Providers.CDSContent
{
  internal class RulesDocument : CDSDocument
  {
    public const int RoutingRulesRequestType = 696;
    private const string RulesDocFormat = "content/{0}/{1}.rule";

    public RulesDocument(IProviderContainer container, string appName, string relativePath)
    {
      Container = container;
      DefaultContentPath = string.Format(RulesDocFormat, appName, relativePath);
      SetContentPath();
    }

    public ReadOnlyCollection<IRoutingRule> GetRoutingRules(string type)
    {
      ReadOnlyCollection<IRoutingRule> routingRules = null;

      var requestData = new CDSRequestData(ContentPath);
      try
      {
        RoutingRulesResponseData responseData = ByPassDataCache ? (RoutingRulesResponseData)Engine.Engine.ProcessRequest(requestData, RoutingRulesRequestType) : (RoutingRulesResponseData)DataCache.DataCache.GetProcessRequest(requestData, RoutingRulesRequestType);

        if (responseData.IsSuccess)
        {
          responseData.TryGetValue(type, out routingRules);
          if (type == RoutingRuleTypes.Redirect)
          {
            LogCDSDebugInfo(responseData);
          }
        }
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException("RulesDocument.GetRoutingRules()", 0, "CDSContentProvider error getting route rules. " + ex.Message, ContentPath));
      }

      return routingRules;
    }
  }
}
