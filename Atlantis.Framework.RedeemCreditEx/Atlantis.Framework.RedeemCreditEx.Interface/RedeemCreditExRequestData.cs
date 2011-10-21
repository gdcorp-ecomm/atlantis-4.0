using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RedeemCreditEx.Interface
{
  public class RedeemCreditExRequestData : RequestData
  {
    private RedeemCreditExRequestData()
      : base("", "", "", "", 0)
    { }

    public RedeemCreditExRequestData( string shopperID, string sourceURL, string orderID, 
                                      string pathway, int pageCount, string redeemXML)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      RedeemXML = redeemXML;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in RedeemCreditExRequestData");
    }

    public string RedeemXML { get; set; }
  }
}
