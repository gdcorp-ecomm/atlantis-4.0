using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.MyaMirageData.Interface
{
  public class MyaMirageDataResponseData : IResponseData, ISessionSerializableResponse
  {
    private readonly HashSet<string> _mirageKeySet;
    private Dictionary<string, int> _mirageData;
    bool _success = true;
    AtlantisException _exception;

    public MyaMirageDataResponseData()
    {
      _mirageData = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
      _mirageKeySet = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
    }

    public MyaMirageDataResponseData(IDictionary<string, int> mirageData)
    {
      _mirageKeySet = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

      if (mirageData != null)
      {
        _mirageData = new Dictionary<string,int>(mirageData, StringComparer.InvariantCultureIgnoreCase);
        foreach (string key in _mirageData.Keys)
        {
          _mirageKeySet.Add(key);
        }
      }
      else
      {
        _mirageData = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
      }
    }

    public MyaMirageDataResponseData(AtlantisException ex)
    {
      _exception = ex;
      _success = false;
    }

    public int GetProductGroupTotal(int productGroupId)
    {
      return GetProductNamespaceTotal("pg|" + productGroupId.ToString());
    }

    public int GetProductIdTotal(int productId)
    {
      return GetProductNamespaceTotal(productId.ToString());
    }

    public int GetProductNamespaceTotal(string productNamespace)
    {
      int result = 0;
      int total;
      if (_mirageData.TryGetValue(productNamespace, out total))
      {
        result = total;
      }
      return result;
    }

    public HashSet<string> AllMirageDataKeys
    {
      get { return _mirageKeySet; }
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sb = new StringBuilder("<data>");

      foreach (string key in _mirageData.Keys)
      {
        sb.Append("<item key=\"");
        sb.Append(key);
        sb.Append("\" value=\"");
        sb.Append(_mirageData[key].ToString());
        sb.Append("\" />");
      }

      sb.Append("</data>");
      return sb.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    #endregion

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      _mirageData = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

      try
      {
        XElement dataElement = XElement.Parse(sessionData);
        foreach (XElement itemElement in dataElement.Descendants("item"))
        {
          string key = itemElement.Attribute("key").Value;
          string value = itemElement.Attribute("value").Value;

          int quantity;
          if ((!string.IsNullOrEmpty(key)) && (Int32.TryParse(value, out quantity)))
          {
            _mirageData[key] = quantity;
          }
        }
      }
      catch (Exception ex)
      {
        throw new AtlantisException("MyaMirageDataResponseData.DeserializeSessionData", "0", ex.Message + ex.StackTrace, sessionData, null, null);
      }
    }

    #endregion
  }
}
