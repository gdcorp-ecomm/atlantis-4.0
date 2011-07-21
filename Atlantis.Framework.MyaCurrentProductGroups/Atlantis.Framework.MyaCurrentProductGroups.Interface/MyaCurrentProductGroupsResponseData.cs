using System;
using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.MyaCurrentProductGroups.Interface
{
  public class MyaCurrentProductGroupsResponseData : IResponseData, IEnumerable<int>
  {
    private HashSet<int> _currentProductGroups;

    public MyaCurrentProductGroupsResponseData(HashSet<int> currentProductGroups)
    {
      if (currentProductGroups == null)
      {
        _currentProductGroups = new HashSet<int>();
      }
      else
      {
        _currentProductGroups = currentProductGroups;
      }
    }

    public MyaCurrentProductGroupsResponseData(RequestData requestData, Exception ex)
    {
      string message = ex.Message + Environment.NewLine + ex.StackTrace;
      RequestException = new AtlantisException(requestData, "MyaCurrentProductGroupsResponseData", message, requestData.ToXML(), ex);
      _currentProductGroups = new HashSet<int>();
    }

    public AtlantisException RequestException { get; private set; }

    #region IResponseData Members

    public string ToXML()
    {
      XElement rootElement = new XElement("MyaCurrentProductGroupsResponseData");
      XDocument xmlDoc = new XDocument(rootElement);

      if (RequestException != null)
      {
        XElement errorElement = new XElement("Error", RequestException.ErrorDescription);
        rootElement.Add(errorElement);
      }

      foreach (int productGroupId in _currentProductGroups)
      {
        XElement groupIdElement = new XElement("ProductGroupId", productGroupId.ToString());
        rootElement.Add(groupIdElement);
      }

      return xmlDoc.ToString();
    }

    public AtlantisException GetException()
    {
      return null;
    }

    #endregion

    #region IEnumerable<int> Members

    public IEnumerator<int> GetEnumerator()
    {
      return _currentProductGroups.GetEnumerator();
    }

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return _currentProductGroups.GetEnumerator();
    }

    #endregion
  }
}
