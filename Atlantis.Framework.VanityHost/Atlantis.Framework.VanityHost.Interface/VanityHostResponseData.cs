using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.VanityHost.Interface
{
  public class VanityHostResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private List<VanityHostItem> _vanityHostItems;

    public VanityHostResponseData(IEnumerable<VanityHostItem> vanityHostItems)
    {
      if (vanityHostItems != null)
      {
        _vanityHostItems = new List<VanityHostItem>(vanityHostItems);
      }
      else
      {
        _vanityHostItems = new List<VanityHostItem>();
      }
    }

    public IEnumerable<VanityHostItem> VanityHostItems
    {
      get
      {
        return _vanityHostItems;
      }
    }

    public VanityHostResponseData(RequestData request, Exception ex)
    {
      _exception = new AtlantisException(request, "VanityHostResponseData.ctor", ex.Message, ex.StackTrace, ex);
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
