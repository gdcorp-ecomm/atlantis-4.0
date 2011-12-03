using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ManagerCategories.Interface
{
  public class ManagerCategoriesResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private HashSet<int> _managerCategories;
    private Dictionary<string, string> _managerAttributes;

    public ManagerCategoriesResponseData(IDictionary<string, string> managerAttributes, IEnumerable<int> managerCategories)
    {
      _managerCategories = new HashSet<int>(managerCategories);
      _managerAttributes = new Dictionary<string, string>(managerAttributes);
    }

    public ManagerCategoriesResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "ManagerCategoriesResponseData", ex.Message, ex.StackTrace, ex);
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

    public bool HasManagerCategory(int category)
    {
      return _managerCategories.Contains(category);
    }

    public bool HasAllManagerCategories(IEnumerable<int> categories)
    {
      return _managerCategories.IsSupersetOf(categories);
    }

    public bool HasAnyManagerCategories(IEnumerable<int> categoies)
    {
      return _managerCategories.Overlaps(categoies);
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
