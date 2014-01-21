using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.Products.Interface
{
  public class OperatingCompany
  {
    internal OperatingCompany(string companyId, string countryCode)
    {
      CompanyId = companyId;
      CountryCode = countryCode;
    }

    public string CountryCode
    {
      get;
      private set;
    }

    public string CompanyId
    {
      get;
      private set;
    }
  }
}
