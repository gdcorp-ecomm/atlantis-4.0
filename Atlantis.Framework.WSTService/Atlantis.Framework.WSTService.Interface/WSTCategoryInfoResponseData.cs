using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WSTService.Interface
{
  public class WSTCategoryInfoResponseData : IResponseData
  {
    private AtlantisException _exception { get; set; }

    public IDictionary<int, string> Categories { get; private set; }

    public bool IsSuccess { get; private set; }

    public WSTCategoryInfoResponseData(IDictionary<int, string> categories)
    {
      IsSuccess = true;
      Categories = categories;
    }

    public WSTCategoryInfoResponseData(RequestData requestData, Exception ex)
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
