using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WSTService.Interface
{
  public class WSTTemplateInfoResponseData : IResponseData
  {
    private AtlantisException _exception { get; set; }

    public IList<WSTTemplateInfo> Templates { get; private set; }

    public bool IsSuccess { get; private set; }

    public WSTTemplateInfoResponseData(IList<WSTTemplateInfo> templates)
    {
      IsSuccess = true;
      Templates = templates;
    }

    public WSTTemplateInfoResponseData(RequestData requestData, Exception ex)
    {
      IsSuccess = false;
      _exception = new AtlantisException(requestData, MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message, ex.StackTrace, ex);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return string.Empty;
    }
  }
}
