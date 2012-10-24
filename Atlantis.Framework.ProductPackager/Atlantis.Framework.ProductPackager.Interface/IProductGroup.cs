using System.Collections.Generic;

namespace Atlantis.Framework.ProductPackager.Interface
{
  public interface IProductGroup
  {
    string Id { get; }

    string Name { get; }

    string GroupType { get; }

    IDictionary<string, IProductGroupAttribute> ProductGroupAttributeDictionary { get; }

    IDictionary<int, IProductGroupPackageData> ProductGroupPackageDataDictionary { get; }
  }
}
