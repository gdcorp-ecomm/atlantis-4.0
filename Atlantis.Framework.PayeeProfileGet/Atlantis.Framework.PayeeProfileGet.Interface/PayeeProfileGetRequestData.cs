using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PayeeProfileGet.Interface
{
  public class PayeeProfileGetRequestData : RequestData
  {

    private int _iCAPID = 0;
    public int ICAPID
    {
      get { return _iCAPID; }
      set { _iCAPID = value; }
    }

    public PayeeProfileGetRequestData(string shopperID,
                            string sourceURL,
                            string orderID,
                            string pathway,
                            int pageCount, int iCAPID)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      this._iCAPID = iCAPID;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("PayeeProfileGet is not a cacheable request.");
    }

  }
}
