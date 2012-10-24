using Atlantis.Framework.ProductPackager.Impl.ProductPackage;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackager.Impl
{
  internal class FbProductPackageChildProduct : IProductPackageChildProduct
  {
    public int ProductId { get; private set; }

    public int Quantity { get; private set; }

    public double Duration { get; private set; }

    public bool IsChild { get; private set; }

    public bool IsFree { get; private set; }

    public string DiscountCode { get; private set; }

    public int DisplayOrder { get; private set; }

    private FbProductPackageChildProduct(string productId, string quantity, string duration, string isChild, string isFree, string discountCode, string displayOrder)
    {
      ProductId = ParseHelper.ParseInt(productId, "ChildProd \"pfid\" is not an integer. Value: {0}");
      Quantity = ParseHelper.ParseInt(quantity, "ChildProd \"qty\" is not an integer. Value: {0}");
      Duration = ParseHelper.ParseDouble(duration, "ChildProd \"duration\" is not an double. Value: {0}");
      IsChild = !string.IsNullOrEmpty(isChild) && ParseHelper.ParseBool(isChild, "ChildProd \"childflag\" is not an bool. Value: {0}");
      IsFree = !string.IsNullOrEmpty(isFree) && ParseHelper.ParseBool(isFree, "ChildProd \"isFree\" is not an bool. Value: {0}");
      DiscountCode = discountCode;
      DisplayOrder = string.IsNullOrEmpty(displayOrder) ? 1 : ParseHelper.ParseInt(displayOrder, "ChildProd \"disporder\" is not an integer. Value: {0}");
    }

    internal FbProductPackageChildProduct(CartChildprod childProduct) : this(childProduct.pfid, childProduct.qty, "1", childProduct.childflag, childProduct.isfree, childProduct.discountcode, "1")
    {
    }

    internal FbProductPackageChildProduct(AddonChildProd childProduct) : this(childProduct.pfid, childProduct.qty, "1", "true", "false", string.Empty, childProduct.disporder)
    {
    }

    internal FbProductPackageChildProduct(UpsellChildProd childProduct) : this(childProduct.pfid, childProduct.qty, "1", "false", "false", string.Empty, string.Empty)
    {
    }
  }
}
