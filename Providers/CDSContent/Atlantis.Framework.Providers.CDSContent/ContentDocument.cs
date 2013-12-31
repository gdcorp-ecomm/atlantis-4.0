using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using System;

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

        if (responseData.IsSuccess && !string.IsNullOrEmpty(responseData.Content))
        {
          contentVersion = responseData;
          LogCDSDebugInfo(responseData);
        }
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException("ContentDocument.GetContent()",
                                                                 "0",
                                                                 "CDSContentProvider error getting content. " + ex.Message,
                                                                 ContentPath,
                                                                 null,
                                                                 null));
      }

      return contentVersion;
    }

    private void LogCDSDebugInfo(ContentVersionResponseData cv)
    {
      try
      {
        IDebugContext dc;
        if (Container.TryResolve<IDebugContext>(out dc))
        {
          int counter = GetDocumentCounter();
          dc.LogDebugTrackingData(counter + ". CDS Version Id", cv._id.oid);
          dc.LogDebugTrackingData(counter + ". CDS Doc Id", cv.DocumentId.oid);
          dc.LogDebugTrackingData(counter + ". CDS Doc Name", cv.Name);
          dc.LogDebugTrackingData(counter + ". CDS Query", cv.Url);
          if (cv.ActiveDate != null)
          {
            dc.LogDebugTrackingData(counter + ". CDS ActiveDate", cv.ActiveDate.ADate ?? string.Empty);
          }
          if (cv.PublishDate != null)
          {
            dc.LogDebugTrackingData(counter + ". CDS PublishDate", cv.PublishDate.ADate ?? string.Empty);
          }
          dc.LogDebugTrackingData(counter + ". CDS Status", cv.Status ? "Published" : "Draft");
          dc.LogDebugTrackingData(counter + ". CDS Published By", cv.User.FullName);
        }
      }
      catch { }
    }
  }
}
