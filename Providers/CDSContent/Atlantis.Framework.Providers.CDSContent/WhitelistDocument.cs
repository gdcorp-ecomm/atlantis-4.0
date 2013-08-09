using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Providers.CDSContent
{
  internal class WhitelistDocument : CDSDocument
  {
    private const string WhiteListFormat = "content/{0}/whitelist";
    public const int UrlWhitelistRequestType = 688;
    public const string VersionIDQueryStringParamName = "whitelist";

    public WhitelistDocument(IProviderContainer container, string appName)
    {
      Container = container;
      RawPath = string.Format(WhiteListFormat, appName);
      AddDocIdParam(VersionIDQueryStringParamName);
    }
       

    public IWhitelistResult CheckWhiteList(string relativePath)
    {
      IWhitelistResult whitelistResult;

      try
      {
        CDSRequestData requestData = new CDSRequestData(ProcessedPath);
        UrlWhitelistResponseData responseData = ByPassDataCache ? (UrlWhitelistResponseData)Engine.Engine.ProcessRequest(requestData, UrlWhitelistRequestType) : (UrlWhitelistResponseData)DataCache.DataCache.GetProcessRequest(requestData, UrlWhitelistRequestType);
        whitelistResult = responseData.CheckWhitelist(relativePath);
      }
      catch (Exception ex)
      {
        whitelistResult = UrlWhitelistResponseData.NullWhitelistResult;

        Engine.Engine.LogAtlantisException(new AtlantisException("WhitelistDocument.CheckWhiteList()",
                                                                 "0",
                                                                 "CDSContentProvider whitelist error. " + ex.Message,
                                                                 ProcessedPath,
                                                                 null,
                                                                 null));
      }

      return whitelistResult;
    }


  }
}
