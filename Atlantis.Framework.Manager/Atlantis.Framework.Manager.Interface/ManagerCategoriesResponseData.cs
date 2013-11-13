using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Manager.Interface
{
  public class ManagerCategoriesResponseData : ResponseData
  {
    private readonly Dictionary<string, string> _managerAttributes;
    private readonly HashSet<int> _managerCategories;

    static ManagerCategoriesResponseData()
    {
      Empty = new ManagerCategoriesResponseData();
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

    public static ManagerCategoriesResponseData Empty { get; private set; }

    public Dictionary<string, string>.KeyCollection ManagerAttributeKeys
    {
      get { return _managerAttributes.Keys; }
    }

    public int ManagerCategoryCount
    {
      get { return _managerCategories.Count; }
    }

    public static ManagerCategoriesResponseData FromCacheDataXml(string cacheDataXml)
    {
      if (string.IsNullOrEmpty(cacheDataXml))
      {
        return Empty;
      }

      var dataElement = XElement.Parse(cacheDataXml);

      var managerAttributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      var managerCategories = new HashSet<int>();

      var userElement = dataElement;
      if (userElement.Name.LocalName != "user")
      {
        userElement = dataElement.Descendants("user").FirstOrDefault();
      }

      if (userElement == null)
      {
        return Empty;
      }

      foreach (var managerAttribute in userElement.Attributes())
      {
        managerAttributes[managerAttribute.Name.LocalName] = managerAttribute.Value;
      }

      IEnumerable<XElement> categoryElements = userElement.Descendants("category");
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

    public bool HasManagerAttribute(string key)
    {
      return _managerAttributes.ContainsKey(key);
    }

    public bool TryGetManagerAttribute(string key, out string value)
    {
      return _managerAttributes.TryGetValue(key, out value);
    }

    public bool HasManagerCategory(int category)
    {
      return _managerCategories.Contains(category);
    }

    public bool HasAllManagerCategories(IEnumerable<int> categories)
    {
      var result = false;
      if (categories != null)
      {
        result = _managerCategories.IsSupersetOf(categories);
      }
      return result;
    }

    public bool HasAnyManagerCategories(IEnumerable<int> categories)
    {
      var result = false;
      if (categories != null)
      {
        result = _managerCategories.Overlaps(categories);
      }
      return result;
    }
  }
}