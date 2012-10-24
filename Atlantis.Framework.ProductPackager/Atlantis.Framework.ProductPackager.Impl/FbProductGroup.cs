using System.Collections.Generic;
using Atlantis.Framework.ProductPackager.Impl.ProductPackage;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackager.Impl
{
  internal class FbProductGroup : IProductGroup
  {
    private readonly ProductGroup _fbProductGroup;

    private string _id;
    public string Id
    {
      get
      {
        if(_id == null)
        {
          _id = _fbProductGroup.prodGroupCode;
        }
        return _id;
      }
    }

    private string _name;
    public string Name
    {
      get
      {
        if(_name == null)
        {
          _name = _fbProductGroup.name;
        }
        return _name;
      }
    }

    private string _groupType;
    public string GroupType
    {
      get
      {
        if(_groupType == null)
        {
          _groupType = _fbProductGroup.groupType;
        }
        return _groupType;
      }
    }

    private IDictionary<string, IProductGroupAttribute> _productGroupAttributeDictionary;
    public IDictionary<string, IProductGroupAttribute> ProductGroupAttributeDictionary
    {
      get
      {
        if(_productGroupAttributeDictionary == null)
        {
          _productGroupAttributeDictionary = PopulateProductGroupAttributeDictionary(_fbProductGroup);
        }
        return _productGroupAttributeDictionary;
      }
    }

    private IDictionary<int, IProductGroupPackageData> _productGroupPackageDataDictionary;
    public IDictionary<int, IProductGroupPackageData> ProductGroupPackageDataDictionary
    {
      get
      {
        if(_productGroupPackageDataDictionary == null)
        {
          _productGroupPackageDataDictionary = PopulateProductGroupPackageDataDictionary(_fbProductGroup);
        }
        return _productGroupPackageDataDictionary;
      }
    }

    internal FbProductGroup(ProductGroup fbProductGroup)
    {
      _fbProductGroup = fbProductGroup;
    }

    private static IDictionary<string, IProductGroupAttribute> PopulateProductGroupAttributeDictionary(ProductGroup fbProductGroup)
    {
      IDictionary<string, IProductGroupAttribute> productGroupAttributeDictionary = new Dictionary<string, IProductGroupAttribute>(fbProductGroup.Attribs.Length);
      
      foreach (ProductGroupAttribute fbProductGroupAttribute in fbProductGroup.Attribs)
      {
        IProductGroupAttribute productGroupAttribute = new FbProductGroupAttribute(fbProductGroupAttribute);
        productGroupAttributeDictionary[productGroupAttribute.Name.ToLowerInvariant()] = productGroupAttribute;
      }

      return productGroupAttributeDictionary;
    }

    private static IDictionary<int, IProductGroupPackageData> PopulateProductGroupPackageDataDictionary(ProductGroup fbProductGroup)
    {
      IDictionary<int, IProductGroupPackageData> productGroupPackageDataDictionary = new Dictionary<int, IProductGroupPackageData>(fbProductGroup.Prods.Length);
      
      foreach (ProductGroupProducts fbProductGroupPackageData in fbProductGroup.Prods)
      {
        IProductGroupPackageData productGroupPackageData = new FbProductGroupPackageData(fbProductGroupPackageData);
        productGroupPackageDataDictionary[productGroupPackageData.ProductId] = productGroupPackageData;
      }

      return productGroupPackageDataDictionary;
    }
  }
}