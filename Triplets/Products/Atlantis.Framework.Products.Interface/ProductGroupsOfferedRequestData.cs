using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Products.Interface
{
  public class ProductGroupsOfferedRequestData : RequestData
  {
    public int PrivateLabelId { get; private set; }
    
    public ProductGroupsOfferedRequestData(int privateLabelId)
    {
      PrivateLabelId = privateLabelId;
    }

    public override string GetCacheMD5()
    {
      return PrivateLabelId.ToString();
    }
  }
}
