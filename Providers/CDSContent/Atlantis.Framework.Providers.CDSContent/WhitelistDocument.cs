using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Providers.CDSContent
{
  internal class WhitelistDocument : CDSDocument
  {
    private const string WhiteListFormat = "content/{0}/whitelist";
    public const int UrlWhitelistRequestType = 688;

    public WhitelistDocument(IProviderContainer container, string appName)
    {
      Container = container;
      DefaultContentPath = string.Format(WhiteListFormat, appName);
      SetContentPath();
    }
       

    public IWhitelistResult CheckWhiteList(string relativePath)
    {
      IWhitelistResult whitelistResult;

      try
      {
        CDSRequestData requestData = new CDSRequestData(ContentPath);
        UrlWhitelistResponseData responseData = ByPassDataCache ? (UrlWhitelistResponseData)Engine.Engine.ProcessRequest(requestData, UrlWhitelistRequestType) : (UrlWhitelistResponseData)DataCache.DataCache.GetProcessRequest(requestData, UrlWhitelistRequestType);
        whitelistResult = responseData.CheckWhitelist(relativePath);
        LogCDSDebugInfo(responseData.Id);
      }
      catch (Exception ex)
      {
        whitelistResult = UrlWhitelistResponseData.NullWhitelistResult;

        Engine.Engine.LogAtlantisException(new AtlantisException("WhitelistDocument.CheckWhiteList()",
                                                                 "0",
                                                                 "CDSContentProvider whitelist error. " + ex.Message,
                                                                 ContentPath,
                                                                 null,
                                                                 null));
      }

      return whitelistResult;
    }

    private void LogCDSDebugInfo(ContentId id)
    {
      try
      {
        IDebugContext dc;
        if (Container.TryResolve<IDebugContext>(out dc))
        {
          dc.LogDebugTrackingData("CDS Whitelist Version Id", id.oid);
        }
      }
      catch { }
    }
  }
}
