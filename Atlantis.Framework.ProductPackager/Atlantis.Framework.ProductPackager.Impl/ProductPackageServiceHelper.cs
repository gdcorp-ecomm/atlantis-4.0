using System;
using Atlantis.Framework.ProductPackager.Impl.ProductPackage;

namespace Atlantis.Framework.ProductPackager.Impl
{
  public static class ProductPackageServiceHelper
  {
    public static Service GetServiceInstance(string url, TimeSpan requestTimeout)
    {
      Service productPackageService = new Service();
      productPackageService.Url = url;
      productPackageService.Timeout = (int)requestTimeout.TotalMilliseconds;

      return productPackageService;
    }
  }
}
