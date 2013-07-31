//using Atlantis.Framework.Brand.Interface;
using Atlantis.Framework.Interface;
//using Atlantis.Framework.Providers.Brand.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Brand
{
  public class CompanyNameData
  {
    private IProviderContainer _container;
//ICompanyName = _company ;

    internal CompanyNameData(IProviderContainer container)
    {
      _container = container;
    }
  }
}
