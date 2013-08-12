using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;
using System;

namespace Atlantis.Framework.Providers.CDSContent
{
  internal class ContentDocument : CDSDocument
  {
    public const int ContentVersionRequestType = 687;
    public static readonly IRenderContent NullRenderContent = new ContentVersionResponseData(null);
    public const string VersionIDQueryStringParamName = "version";

    public ContentDocument(IProviderContainer container, string rawPath)
    {
      Container = container;
      RawPath = rawPath;
      AddDocIdParam(VersionIDQueryStringParamName);
    }

    public IRenderContent GetContent()
    {
      IRenderContent contentVersion = NullRenderContent;

      try
      {
        var requestData = new CDSRequestData(ProcessedPath);
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
                                                                 ProcessedPath,
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
          dc.LogDebugTrackingData("CDS Version Id", cv._id.oid);
          dc.LogDebugTrackingData("CDS Doc Id", cv.DocumentId.oid);
          dc.LogDebugTrackingData("CDS Doc Name", cv.Name);
          dc.LogDebugTrackingData("CDS Query", cv.Url);
          if (cv.ActiveDate != null)
          {
            dc.LogDebugTrackingData("CDS ActiveDate", cv.ActiveDate.ADate ?? string.Empty);
          }
          if (cv.PublishDate != null)
          {
            dc.LogDebugTrackingData("CDS PublishDate", cv.PublishDate.ADate ?? string.Empty);
          }
          dc.LogDebugTrackingData("CDS Status", cv.Status ? "Published" : "Draft");
          dc.LogDebugTrackingData("CDS Published By", cv.User.FullName);
        }
      }
      catch { }
    }
  }
}
