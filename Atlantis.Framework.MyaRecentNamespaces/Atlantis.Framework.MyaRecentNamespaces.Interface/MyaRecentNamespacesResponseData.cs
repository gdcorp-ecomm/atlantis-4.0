using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.MyaRecentNamespaces.Interface
{
  public class MyaRecentNamespacesResponseData : IResponseData, ISessionSerializableResponse
  {
    #region Properties
    private AtlantisException _exception;
    private HashSet<string> _namespaces;

    public HashSet<string> Namespaces
    {
      get { return _namespaces; }
    }
    #endregion

    public MyaRecentNamespacesResponseData()
    { }

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

    public MyaRecentNamespacesResponseData(Exception ex, RequestData request)
    {
      _exception = new AtlantisException(request, "MyaRecentNamespacesResponseData", ex.Message + ex.StackTrace, request.ShopperID);
    }

    #region IResponseData Members

    public string ToXML()
    {
      XElement namespacesElements = new XElement("namespaces");
      foreach (string ns in Namespaces)
      {
        namespacesElements.Add(new XElement("namespace", ns));
      }

      return namespacesElements.ToString();

    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    #endregion

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      HashSet<string> namespaces = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
      XDocument xDoc = XDocument.Parse(sessionData);
      foreach (XElement ns in xDoc.Element("namespaces").Elements())
      {
        namespaces.Add(ns.Value.ToString());
      }

      _namespaces = namespaces;
    }
    #endregion
  }
}
