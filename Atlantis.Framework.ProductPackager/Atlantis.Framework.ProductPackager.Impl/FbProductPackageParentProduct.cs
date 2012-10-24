using System.Collections.Generic;
using Atlantis.Framework.ProductPackager.Impl.ProductPackage;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackager.Impl
{
  internal class FbProductPackageParentProduct : IProductPackageParentProduct
  {
    public int ProductId { get; private set; }
    
    public int Quantity { get; private set; }
    
    public double Duration { get; private set; }
    
    public string DiscountCode { get; private set; }
    
    public string DisplayOverride { get; private set; }
    
    public bool IsParent { get; private set; }
    
    public IList<IProductPackageChildProduct> ChildProducts { get; private set; }

    private FbProductPackageParentProduct(string productId, string quantity, string duration, string discountCode, string displayOverride, string isParent)
    {
      ProductId = ParseHelper.ParseInt(productId, "ParentProd \"pfid\" is not an integer. Value: {0}");
      Quantity = ParseHelper.ParseInt(quantity, "ParentProd \"qty\" is not an integer. Value: {0}");
      Duration = ParseHelper.ParseDouble(duration, "ParentProd \"duration\" is not an double. Value: {0}");
      DiscountCode = discountCode;
      DisplayOverride = displayOverride;
      IsParent = !string.IsNullOrEmpty(isParent) && ParseHelper.ParseBool(isParent, "CartParentProd \"parentflag\" is not an bool. Value: {0}");
    }

    internal FbProductPackageParentProduct(CartParentProd parentProduct) : this(parentProduct.pfid, parentProduct.qty, "1", parentProduct.discountcode, parentProduct.dispoverride, parentProduct.parentflag)
    {
      List<FbProductPackageChildProduct> childProducts = new List<FbProductPackageChildProduct>(parentProduct.ChildProds.Length);

      foreach (CartChildprod childProd in parentProduct.ChildProds)
      {
        childProducts.Add(new FbProductPackageChildProduct(childProd));
      }

      SetChildProducts(childProducts);
    }

    internal FbProductPackageParentProduct(AddonParentProd parentProduct) : this(parentProduct.pfid, parentProduct.qty, "1", string.Empty, string.Empty, "false")
    {
      List<FbProductPackageChildProduct> childProducts = new List<FbProductPackageChildProduct>(parentProduct.ChildProds.Length);

      foreach (AddonChildProd childProd in parentProduct.ChildProds)
      {
        childProducts.Add(new FbProductPackageChildProduct(childProd));
      }

      SetChildProducts(childProducts);
    }

    internal FbProductPackageParentProduct(UpsellParentProd parentProduct) : this(parentProduct.pfid, parentProduct.qty, "1", string.Empty, string.Empty, "false")
    {
      List<FbProductPackageChildProduct> childProducts = new List<FbProductPackageChildProduct>(parentProduct.ChildProds.Length);

      foreach (UpsellChildProd childProd in parentProduct.ChildProds)
      {
        childProducts.Add(new FbProductPackageChildProduct(childProd));
      }

      SetChildProducts(childProducts);
    }

    private void SetChildProducts(List<FbProductPackageChildProduct> childProducts)
    {
      childProducts.Sort(new DisplayOrderComparer());
      ChildProducts = new List<IProductPackageChildProduct>(childProducts);
    }

    private class DisplayOrderComparer : IComparer<FbProductPackageChildProduct>
    {
      public int Compare(FbProductPackageChildProduct x, FbProductPackageChildProduct y)
      {
        return x.DisplayOrder.CompareTo(y.DisplayOrder);
      }
    }
  }
}