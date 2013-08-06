using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.Brand.Interface
{
  public interface IBrandProvider
  {
    string GetCompanyName(string companyPropertyKey);
    string GetProductLineName(string productLineKey, int contextId = 0);
  }
}
