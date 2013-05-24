using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Template.Interface;

namespace Atlantis.Framework.CDS.Interface
{
  public class RenderTemplateContentResponseData : CDSResponseData
  {
    public IRenderTemplateContent RenderTemplateContent { get; private set; }

    public RenderTemplateContentResponseData(string responseData, IRenderTemplateContent renderTemplateContent) : base(responseData)
    {
      RenderTemplateContent = renderTemplateContent;
    }

    public RenderTemplateContentResponseData(RequestData requestData, Exception exception) : base(requestData, exception)
    {
    }
  }
}
