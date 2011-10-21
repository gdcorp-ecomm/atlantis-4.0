using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetCreditGroupSummary.Interface
{
  public class GetCreditGroupSummaryRequestData : RequestData
  {

    private GetCreditGroupSummaryRequestData()
      : base("", "", "", "", 0)
    { }

    public GetCreditGroupSummaryRequestData(string shopperID, string sourceURL, string orderID, 
                                            string pathway, int pageCount, int displayGroupID)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      DisplayGroupID = displayGroupID;
    }

    public override string GetCacheMD5()
    {
      return string.Empty;
    }

    public int DisplayGroupID { get; set; }
  }
}
