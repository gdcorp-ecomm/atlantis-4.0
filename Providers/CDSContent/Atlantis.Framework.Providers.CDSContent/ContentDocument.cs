using System;

using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Providers.CDSContent
{
  internal class ContentDocument : CDSDocument
  {
    public const int ContentVersionRequestType = 687;
    public static readonly IRenderContent NullRenderContent = new ContentVersionResponseData(null);

    public ContentDocument(IProviderContainer container, string defaultContentPath)
    {
      Container = container;
      DefaultContentPath = defaultContentPath;
      SetContentPath();
    }

    public IRenderContent GetContent()
    {
      IRenderContent contentVersion = NullRenderContent;

      try
      {
        var requestData = new CDSRequestData(ContentPath);
        ContentVersionResponseData responseData = ByPassDataCache ? (ContentVersionResponseData)Engine.Engine.ProcessRequest(requestData, ContentVersionRequestType) : (ContentVersionResponseData)DataCache.DataCache.GetProcessRequest(requestData, ContentVersionRequestType);

        if (responseData.IsSuccess && (!string.IsNullOrEmpty(responseData.Content) || CdsContentProviderGlobalSettings.AllowEmptyContent))
        {
          contentVersion = responseData;
          LogCDSDebugInfo(responseData);
        }
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException("ContentDocument.GetContent()", 0, "CDSContentProvider error getting content. " + ex.Message, ContentPath));
      }

      return contentVersion;
    }
  }
}
