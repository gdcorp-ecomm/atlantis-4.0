using Atlantis.Framework.Products.Interface;

namespace Atlantis.Framework.Providers.Products.Handlers
{
  internal interface IProductGroupOfferedHandler
  {
    bool IsProductGroupOffered(int productGroupType, ProductGroupsOfferedResponseData responseData);
  }
}
