
namespace Atlantis.Framework.BillingAlertsByShopper.Interface
{

  public class BillingAlert
  {
    private int _billingFailureResourceId;
    private int _setupResourceId;
    private int _expiringResourceId;
    private int _productGroupId;

    /// <summary>
    /// ResourceId of the first account that failed billing
    /// </summary>
    public int BillingFailureResourceId
    {
      get { return _billingFailureResourceId; }     
    }

    /// <summary>
    /// ResourceId of the first account that isn't setup
    /// </summary>
    public int SetupResourceId
    {
      get { return _setupResourceId; }
    }

    /// <summary>
    /// ResourceId of the first account that is going to expire soon
    /// </summary>
    public int ExpiringResourceId
    {
      get { return _expiringResourceId; }
    }   

    public int ProductGroupId
    {
      get { return _productGroupId; }
    }

    public BillingAlert(int productGroupId
      , int billingFailureResourceId
      , int setupResourceId
      , int expiringResourceId )
    {
      _productGroupId = productGroupId;
      _billingFailureResourceId = billingFailureResourceId;
      _setupResourceId = setupResourceId;
      _expiringResourceId = expiringResourceId;     
    }

  }
}
