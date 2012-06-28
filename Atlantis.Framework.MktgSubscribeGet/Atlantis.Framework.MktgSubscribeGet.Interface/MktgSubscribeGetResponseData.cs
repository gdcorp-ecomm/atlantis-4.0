using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MktgSubscribeGet.Interface
{
  public class MktgSubscribeGetResponseData : IResponseData
  {
    private readonly AtlantisException _exception;

    public IDictionary<int, MktgSubscribeOptIn> OptInDictionary { get; private set; }

    public MktgSubscribeGetResponseData(IDictionary<int, MktgSubscribeOptIn> optInDictionary)
    {
      OptInDictionary = optInDictionary;
    }

    public MktgSubscribeGetResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "MktgSubscribeGetResponseData", ex.Message, ex.StackTrace, ex);
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
