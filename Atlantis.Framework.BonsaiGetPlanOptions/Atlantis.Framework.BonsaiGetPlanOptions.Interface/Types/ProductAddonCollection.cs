using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.BonsaiGetPlanOptions.Interface.Types
{
  [DataContract]
  public class ProductAddonCollection : IEnumerable<ProductAddon>
  {
    [DataMember(Name = "Addons")]
    private readonly List<ProductAddon> _addons = new List<ProductAddon>();

    public int Count
    {
      get { return _addons.Count; }
    }

    public ProductAddon Owned
    {
      get { return _addons.Find(addon => addon.IsCurrent); }
    }

    public void Add(ProductAddon item)
    {
      _addons.Add(item);
    }

    public List<ProductAddon> ToList()
    {
      return new List<ProductAddon>(_addons);
    }

    public IEnumerator<ProductAddon> GetEnumerator()
    {
      return _addons.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
