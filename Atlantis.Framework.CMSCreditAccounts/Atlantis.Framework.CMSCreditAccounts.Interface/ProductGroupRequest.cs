
namespace Atlantis.Framework.CMSCreditAccounts.Interface
{
  public class ProductGroupRequest
  {
    public int ProductGroupID { get; set; }
    public bool GetCredits { get; set; }
    public bool GetAccounts { get; set; }
    
    public ProductGroupRequest(int productGroupID,bool getCredits,bool getAccounts)
    {
      ProductGroupID = productGroupID;
      GetAccounts = getAccounts;
      GetCredits = getCredits;
    }

    public ProductGroupRequest()
    {
      GetAccounts = true;
      GetCredits = true;
    }
  }
}
