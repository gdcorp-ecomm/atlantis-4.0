using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductPackagerAddToCartHandler
{
  public delegate ICollection<KeyValuePair<string, string>> AddOrderLevelAttributesDelegate(IProviderContainer providerContainer);
  public delegate ICollection<KeyValuePair<string, string>> AddItemLevelAttributesDelegate(IProviderContainer providerContainer, int unifiedProductId);
}
