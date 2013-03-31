using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.ManagerCategories.Interface
{
  public class ManagerCategoriesResponseData : IResponseData
  {
    public static ManagerCategoriesResponseData Empty { get; private set; }

    static ManagerCategoriesResponseData()
    {
      Empty = new ManagerCategoriesResponseData();
    }

    private HashSet<int> _managerCategories;
    private Dictionary<string, string> _managerAttributes;

    public static ManagerCategoriesResponseData FromCacheDataXml(string cacheDataXml)
    {
      if (string.IsNullOrEmpty(cacheDataXml))
      {
        return Empty;
      }

      try
      {
        XElement dataElement = XElement.Parse(cacheDataXml);

        Dictionary<string, string> managerAttributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        HashSet<int> managerCategories = new HashSet<int>();

        XElement userElement = dataElement;
        if (userElement.Name.LocalName != "user")
        {
          userElement = dataElement.Descendants("user").FirstOrDefault();
        }

        foreach (XAttribute managerAttribute in userElement.Attributes())
        {
          managerAttributes[managerAttribute.Name.LocalName] = managerAttribute.Value;
        }

        var categoryElements = userElement.Descendants("category");
        foreach (var categoryElement in categoryElements)
        {
          int category;
          if (int.TryParse(categoryElement.Value, out category))
          {
            managerCategories.Add(category);
          }
        }

        return new ManagerCategoriesResponseData(managerAttributes, managerCategories);
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException("ManagerCategoriesResponseData.FromCacheDataXml", "0", ex.Message + ex.StackTrace, cacheDataXml, null, null);
        Engine.Engine.LogAtlantisException(exception);
        return Empty;
      }

    }

    private ManagerCategoriesResponseData()
    {
      _managerCategories = new HashSet<int>();
      _managerAttributes = new Dictionary<string, string>();
    }

    private ManagerCategoriesResponseData(Dictionary<string, string> managerAttributes, HashSet<int> managerCategories)
    {
      _managerCategories = managerCategories;
      _managerAttributes = managerAttributes;
    }

    public bool HasManagerAttribute(string key)
    {
      return _managerAttributes.ContainsKey(key);
    }

    public bool TryGetManagerAttribute(string key, out string value)
    {
      return _managerAttributes.TryGetValue(key, out value);
    }

    public Dictionary<string, string>.KeyCollection ManagerAttributeKeys
    {
      get { return _managerAttributes.Keys; }
    }

    public int ManagerCategoryCount
    {
      get { return _managerCategories.Count; }
    }

    public bool HasManagerCategory(int category)
    {
      return _managerCategories.Contains(category);
    }

    public bool HasAllManagerCategories(IEnumerable<int> categories)
    {
      bool result = false;
      if (categories != null)
      {
        result = _managerCategories.IsSupersetOf(categories);
      }
      return result;
    }

    public bool HasAnyManagerCategories(IEnumerable<int> categoies)
    {
      bool result = false;
      if (categoies != null)
      {
        result = _managerCategories.Overlaps(categoies);
      }
      return result;
    }

    public string ToXML()
    {
      return new XElement("ManagerCategoriesResponseData").ToString();
    }

    public AtlantisException GetException()
    {
      return null;
    }

  }
}
