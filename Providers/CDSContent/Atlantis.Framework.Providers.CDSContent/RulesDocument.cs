using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Collections.ObjectModel;

namespace Atlantis.Framework.Providers.CDSContent
{
  internal class RulesDocument : CDSDocument
  {
    public const int RoutingRulesRequestType = 696;
    private const string RulesDocFormat = "content/{0}/{1}.rule";
    public const string VersionIDQueryStringParamName = "rules";

    public RulesDocument(IProviderContainer container, string appName, string relativePath)
    {
      Container = container;
      RawPath = string.Format(RulesDocFormat, appName, relativePath);
      AddDocIdParam(VersionIDQueryStringParamName);
    }

    public ReadOnlyCollection<IRoutingRule> GetRoutingRules(string type)
    {
      ReadOnlyCollection<IRoutingRule> routingRules = null;

      var requestData = new CDSRequestData(ProcessedPath);
      try
      {
        RoutingRulesResponseData responseData = ByPassDataCache ? (RoutingRulesResponseData)Engine.Engine.ProcessRequest(requestData, RoutingRulesRequestType) : (RoutingRulesResponseData)DataCache.DataCache.GetProcessRequest(requestData, RoutingRulesRequestType);

        if (responseData.IsSuccess)
        {
          responseData.TryGetValue(type, out routingRules);
        }
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException("RulesDocument.GetRoutingRules()",
                                                                 "0",
                                                                 "CDSContentProvider error getting route rules. " + ex.Message,
                                                                 ProcessedPath,
                                                                 null,
                                                                 null));
      }

      return routingRules;
    }
  }
}
