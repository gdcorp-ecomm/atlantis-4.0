using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductUpgradePath.Interface
{
  public class ProductUpgradePathRequestData : RequestData
  {

    public int ProductID { get; set; }
    public int PrivateLabelID { get; set; }

    private List<ProductOptions> _productOptions = new List<ProductOptions>();
    public List<ProductOptions> ProductOptions
    {
      get
      {
        return _productOptions;
      }
      set
      {
        _productOptions = value;
      }
    }

    public ProductUpgradePathRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  int productID,
                                  int privateLabelID)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      PrivateLabelID = privateLabelID;
      ProductID = productID;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();

      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(string.Concat(ProductID,":",PrivateLabelID));

      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);

      string sValue = BitConverter.ToString(md5Bytes, 0);

      return sValue.Replace("-", "");
    }


  }
}
