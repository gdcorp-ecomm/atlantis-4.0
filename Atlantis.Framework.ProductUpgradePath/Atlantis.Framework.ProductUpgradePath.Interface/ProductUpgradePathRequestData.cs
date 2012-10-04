using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductUpgradePath.Interface
{
  public class ProductUpgradePathRequestData : RequestData
  {

    public ProductUpgradePathRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    { }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in ProductUpgradePathRequestData");     
    }


  }
}
