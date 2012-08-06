using System.Collections.Generic;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData
{
  public class ProductMap
  {
    #region ReadOnly Properties
    private readonly int _group;
    public int Group
    {
      get { return _group; }
    }
    private readonly HashSet<int> _types;
    public HashSet<int> TypeList
    {
      get { return _types; }
    }
    private readonly string _description;
    public string Description
    {
      get { return _description; }
    }
    #endregion

    internal ProductMap(int group, HashSet<int> types, string description)
    {
      _group = group;
      _types = types;
      _description = description;
    }

    public ProductMap()
    {}
  }
}