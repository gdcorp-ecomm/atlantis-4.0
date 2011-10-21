using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetParentResourcesByAvailableCredits.Interface
{
  public class GetParentResourcesByAvailableCreditsRequestData : RequestData
  {
    private GetParentResourcesByAvailableCreditsRequestData()
      : base("", "", "", "", 0)
    { }

    public GetParentResourcesByAvailableCreditsRequestData(string shopperID, string sourceURL,
      string orderID, string pathway, int pageCount, int childUnifiedProductID,
      int parentDisplayGroupID, string creditType)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      ChildUnifiedProductID = childUnifiedProductID;
      ParentDisplayGroupID = parentDisplayGroupID;
      CreditType = creditType;
    }

    public override string GetCacheMD5()
    {
      return string.Empty;
    }

    public int ChildUnifiedProductID { get; set; }
    public int ParentDisplayGroupID { get; set; }
    public string CreditType { get; set; }
  }
}
