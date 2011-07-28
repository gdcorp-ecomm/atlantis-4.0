using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductOffer.Interface
{
  public class ProductOfferRequestData : RequestData
  {
    const string GETPRODUCTOFFERINGSBYPLID_FORMAT 
      = "<GetProductOfferingsByPLID><param name=\"n_privateLabelID\" value=\"{0}\"/></GetProductOfferingsByPLID>";

    int _privateLabelId = 0;

    public ProductOfferRequestData(string sShopperID,
                                   string sSourceURL,
                                   string sOrderID,
                                   string sPathway,
                                   int iPageCount,
                                   int iPrivateLabelID)
                                   : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      _privateLabelId = iPrivateLabelID;
    }

    public int PrivateLabelID
    {
      get { return _privateLabelId; }
    }

    #region RequestData Members

    public override string ToXML()
    {
      return String.Format(GETPRODUCTOFFERINGSBYPLID_FORMAT, _privateLabelId);
    }

    public override string GetCacheMD5()
    {
      MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
      md5Provider.Initialize();
      byte [] md5Bytes = md5Provider.ComputeHash(BitConverter.GetBytes(_privateLabelId));
      return BitConverter.ToString(md5Bytes).Replace("-", "");
    }

    #endregion

  }
}
