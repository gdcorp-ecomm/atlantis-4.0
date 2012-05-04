using System.Collections.Specialized;

namespace Atlantis.Framework.VanityHost.Interface
{
  public class VanityHostItem
  {
    private NameValueCollection _queryMap;

    public int LinkContextId { get; private set; }
    public string Domain { get; private set; }
    public string DotType { get; private set; }
    public string LinkName { get; private set; }
    public string Redirect { get; private set; }
    public bool IsPermanentRedirect { get; private set; }

    public VanityHostItem(string domain, string dotType, string linkName, string redirect, bool isPermanentRedirect, int linkContextId)
      : this(domain, dotType, linkName, redirect, isPermanentRedirect, linkContextId, null)
    {
    }

    public VanityHostItem(string domain, string dotType, string linkName, string redirect, bool isPermanentRedirect, int linkContextId, NameValueCollection queryMap) 
    {
      Domain = domain;
      DotType = dotType;
      LinkName = linkName;
      Redirect = redirect;
      IsPermanentRedirect = isPermanentRedirect;
      LinkContextId = linkContextId;

      if (queryMap != null)
      {
        _queryMap = new NameValueCollection(queryMap);
      }
      else
      {
        _queryMap = new NameValueCollection();
      }
    }

    public void SetQueryItems(NameValueCollection queryMap)
    {
      if ((queryMap != null) && (_queryMap.Count > 0))
      {
        foreach (string key in _queryMap.Keys)
        {
          queryMap[key] = _queryMap[key];
        }
      }
    }

    public bool HasQueryItems
    {
      get
      {
        return (_queryMap.Count > 0);
      }
    }

  }
}
