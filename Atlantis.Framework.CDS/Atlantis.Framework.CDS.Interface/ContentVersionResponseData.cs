using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Newtonsoft.Json;
using System;

namespace Atlantis.Framework.CDS.Interface
{
  public class ContentVersionResponseData : CDSResponseData, IRenderContent, ICDSDebugInfo
  {
    const string DEBUG_KEY = "Content";

    private readonly ContentVersion _contentVersion;
    public string DebugKey { get { return DEBUG_KEY; } }

    #region Content Version

    public string ContentType { get { return (_contentVersion != null) ? _contentVersion.ContentType : null; } }
    public string Name { get { return (_contentVersion != null) ? _contentVersion.Name : null; } }
    public ContentId _id { get { return (_contentVersion != null) ? _contentVersion._id : null; } }
    public ContentId DocumentId { get { return (_contentVersion != null) ? _contentVersion.DocumentId : null; } }
    public string Url { get { return (_contentVersion != null) ? _contentVersion.Url : null; } }
    public bool Status { get { return (_contentVersion != null) ? _contentVersion.Status : false; } }
    public ContentDate ActiveDate { get { return (_contentVersion != null) ? _contentVersion.ActiveDate : null; } }
    public ContentDate PublishDate { get { return (_contentVersion != null) ? _contentVersion.PublishDate : null; } }
    public ContentUser User { get { return (_contentVersion != null) ? _contentVersion.User : null; } }

    #endregion

    public string Content { get { return (_contentVersion != null) ? _contentVersion.Content : string.Empty; } }

    public ContentVersionResponseData(string responseData) : base(responseData)
    {
      if (!string.IsNullOrEmpty(responseData))
      {
        _contentVersion = JsonConvert.DeserializeObject<ContentVersion>(responseData);
      }
    }

    public ContentVersionResponseData(RequestData requestData, Exception exception) : base(requestData, exception)
    {
    }

    public ContentId VersionId
    {
      get { return _id; }
    }
  }
}
