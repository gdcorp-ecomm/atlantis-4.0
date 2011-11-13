using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaRecentNamespaces.Interface
{
  public class MyaRecentNamespacesResponseData : IResponseData
  {
    private AtlantisException _exception;
    private HashSet<string> _namespaces;

    public MyaRecentNamespacesResponseData(HashSet<string> namespaces)
    {
      if (namespaces == null)
      {
        _namespaces = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
      }
      else
      {
        _namespaces = namespaces;
      }
    }

    public HashSet<string> Namespaces
    {
      get { return _namespaces; }
    }

    public MyaRecentNamespacesResponseData(Exception ex, RequestData request)
    {
      _exception = new AtlantisException(request, "MyaRecentNamespacesResponseData", ex.Message + ex.StackTrace, request.ShopperID);
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
