using System;
using System.Data;
using System.Globalization;

namespace Atlantis.Framework.MYAGetProduct.Interface.Products
{
  public class SocialDomainProduct : MyaProduct
  {
    public SocialDomainProduct() { }

    public override MyaProduct PopulateObjectFromDB(IDataReader dr, MYAGetProductRequestData myaProductRequestData, bool hasIsFreeColumn)
    {
      int workerOutInt = 0; 
      MyaProduct sdpl = new SocialDomainProduct();

      sdpl = base.PopulateObjectFromDB(dr, myaProductRequestData, hasIsFreeColumn);

      sdpl.CommonName = Convert.ToString(dr["commonName"], CultureInfo.CurrentCulture).Trim();
      sdpl.ObsoleteResourceId = int.TryParse(dr["obsoleteResourceID"].ToString(), out workerOutInt) ? (int?)workerOutInt : (int?)Convert.ToInt32(dr["resource_id"]);

      if (dr["isFreeWithME"] != DBNull.Value)
      {
        sdpl.IsFree = Convert.ToInt32(dr["isFreeWithME"]) == 1;
      }

      return sdpl;
    }
  }
}
