using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.BonsaiGetPlanOptions.Interface.Types
{
  [DataContract]
  public class CategoryAddonCollection : IEnumerable<KeyValuePair<int, ProductAddonCollection>>
  {
    [DataMember(Name = "CatAddons")]
    private readonly Dictionary<int, ProductAddonCollection> _categoryAddons = new Dictionary<int, ProductAddonCollection>();

    public ProductAddonCollection this[int categoryId]
    {
      get { return _categoryAddons[categoryId]; }
      set { _categoryAddons[categoryId] = value; }
    }
    
    public void AddProductAddons(int categoryId, ProductAddonCollection addonCollection)
    {
      _categoryAddons.Add(categoryId, addonCollection);
    }
    
    public void AddProductAddon(ProductAddon addon)
    {
      if (!_categoryAddons.ContainsKey(addon.CategoryId))
      {
        _categoryAddons.Add(addon.CategoryId, new ProductAddonCollection());
      }

      this[addon.CategoryId].Add(addon);
    }

    public bool HasKey(int categoryId)
    {
      return _categoryAddons.ContainsKey(categoryId);
    }

    public IEnumerator<KeyValuePair<int, ProductAddonCollection>> GetEnumerator()
    {
      return _categoryAddons.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
