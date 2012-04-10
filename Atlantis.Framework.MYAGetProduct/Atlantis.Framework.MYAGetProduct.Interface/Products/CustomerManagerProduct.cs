using System.Xml;

namespace Atlantis.Framework.MYAGetProduct.Interface.Products
{
  public class CustomerManagerProduct : SocialDomainProduct
  {
    public CustomerManagerProduct() { }

    public override MyaProduct PopulateObjectFromDB(System.Data.IDataReader dr, MYAGetProductRequestData myaProductRequestData, bool hasIsFreeColumn)
    {
      MyaProduct cmp = new CustomerManagerProduct();

      cmp = base.PopulateObjectFromDB(dr, myaProductRequestData, hasIsFreeColumn);

      cmp.CanRenewFreeProducts = false;

      return cmp;
    }
  }
}
