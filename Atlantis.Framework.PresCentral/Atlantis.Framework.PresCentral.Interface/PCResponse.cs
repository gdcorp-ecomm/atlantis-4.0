using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.PresCentral.Interface
{
  public class PCResponse
  {
    XDocument _responseDoc = null;
    private int _resultCode;
    private string _cacheKey = null;
    private Dictionary<string, string> _debugData;
    private List<PCResponseError> _errors;
    private List<PCContentItem> _itemsList;
    private Dictionary<string, PCContentItem> _itemsDictionary;

    public PCResponse(string responseXml)
    {
      _responseDoc = XDocument.Parse(responseXml);
      ParsePCResultCode();
      ParseDebugData();
      ParseCacheKey();
      ParseErrors();
      ParseContentItems();
    }

    private void ParseContentItems()
    {
      _itemsList = new List<PCContentItem>(6);
      _itemsDictionary = new Dictionary<string, PCContentItem>(6, StringComparer.InvariantCultureIgnoreCase);

      XElement itemsElement = _responseDoc.Descendants("items").FirstOrDefault();
      if (itemsElement != null)
      {
        foreach (XElement contentItem in itemsElement.Descendants("item"))
        {
          if (contentItem != null)
          {
            PCContentItem item = new PCContentItem(contentItem);
            _itemsList.Add(item);
            _itemsDictionary[item.Name] = item;
          }
        }
      }
    }

    private void ParseErrors()
    {
      _errors = new List<PCResponseError>();

      XElement errorsElement = _responseDoc.Descendants("errors").FirstOrDefault();
      if (errorsElement != null)
      {
        foreach (XElement errorItem in errorsElement.Descendants("error"))
        {
          if (errorItem != null)
          {
            PCResponseError error = new PCResponseError(errorItem);
            _errors.Add(error);
          }
        }
      }
    }

    private void ParseCacheKey()
    {
      _cacheKey = null;

      XAttribute cacheKeyAtt = _responseDoc.Root.Attribute("cachekey");
      if (cacheKeyAtt != null)
      {
        _cacheKey = cacheKeyAtt.Value;
      }
    }

    private void ParsePCResultCode()
    {
      _resultCode = -1;
      XAttribute resultAtt = _responseDoc.Root.Attribute("result");
      if (resultAtt != null)
      {
        string resultString = resultAtt.Value;
        int resultCode;
        if (int.TryParse(resultString, out resultCode))
        {
          _resultCode = resultCode;
        }
      }
    }

    private void ParseDebugData()
    {
      _debugData = new Dictionary<string, string>();

      XElement debugElement = _responseDoc.Descendants("debug").FirstOrDefault();
      if (debugElement != null)
      {
        foreach (var itemElement in debugElement.Descendants("item"))
        {
          XAttribute nameAtt = itemElement.Attribute("name");
          if ((nameAtt != null) && (!string.IsNullOrEmpty(nameAtt.Value)))
          {
            _debugData[nameAtt.Value] = itemElement.Value;
          }
        }
      }
    }

    public int ResultCode
    {
      get { return _resultCode; }
    }

    public string CacheKey
    {
      get { return _cacheKey; }
    }

    public string GetDebugData(string key)
    {
      string result = null;
      if (!_debugData.TryGetValue(key, out result))
      {
        result = null;
      }
      return result;
    }

    public IEnumerable<string> DebugDataKeys
    {
      get { return _debugData.Keys as IEnumerable<string>; }
    }

    public IEnumerable<PCResponseError> Errors
    {
      get { return _errors as IEnumerable<PCResponseError>; }
    }

    public IEnumerable<PCContentItem> FindContentByLocation(string location)
    {
      return _itemsList.FindAll(ci => ci.Location.Equals(location, StringComparison.InvariantCultureIgnoreCase));
    }

    public PCContentItem FindContentByName(string name)
    {
      PCContentItem result = null;
      if (!_itemsDictionary.TryGetValue(name, out result))
      {
        result = null;
      }

      return result;
    }

    public string ToXML()
    {
      string result = null;
      if (_responseDoc != null)
      {
        result = _responseDoc.ToString(SaveOptions.DisableFormatting);
      }
      return result;
    }

  }


}
