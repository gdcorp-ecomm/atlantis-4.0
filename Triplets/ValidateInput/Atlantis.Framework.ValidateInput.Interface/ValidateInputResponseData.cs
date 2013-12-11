using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ValidateInput.Interface;

namespace Atlantis.Framework.ValidateInput.Interface
{
  public abstract class ValidateInputResponseData : IResponseData
  {
    private readonly AtlantisException _exception;

    public ValidateInputResult Result { get; private set; }

    protected ValidateInputResponseData(ValidateInputResult result)
    {
      Result = result;
    }

    protected ValidateInputResponseData(ValidateInputResult result, RequestData requestData, string sourceFunction, Exception ex)
    {
      Result = result;
      _exception = new AtlantisException(requestData, sourceFunction, ex.Message, requestData.ToXML());
    }

    public virtual string ToXML()
    {
      return string.Empty;
    }

    public virtual AtlantisException GetException()
    {
      return _exception;
    }
  }
}
