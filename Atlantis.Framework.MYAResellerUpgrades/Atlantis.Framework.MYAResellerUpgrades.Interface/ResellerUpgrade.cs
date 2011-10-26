
namespace Atlantis.Framework.MYAResellerUpgrades.Interface
{
  public class ResellerUpgrade
  {
    private int _productId;
    private string _description;

    public int ProductId
    {
      get { return _productId; }
      set { _productId = value; }
    }

    public string Description
    {
      get { return _description; }
      set { _description = value; }
    }
  }
}
