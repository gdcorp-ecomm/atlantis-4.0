using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetPayeeProfile.Interface
{
  public class GetPayeeProfileRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(20);

    private int _iCAPID = 0;
    public int ICAPID
    {
      get { return _iCAPID; }
      set { _iCAPID = value; }
    }

    public GetPayeeProfileRequestData(string shopperID,
                            string sourceURL,
                            string orderID,
                            string pathway,
                            int pageCount, int iCAPID)
      : this(shopperID, sourceURL, orderID, pathway, pageCount,iCAPID, _defaultRequestTimeout)
    {    
    }

    public GetPayeeProfileRequestData(string shopperID,
                            string sourceURL,
                            string orderID,
                            string pathway,
                            int pageCount, int iCAPID, TimeSpan requestTimeout )
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      this._iCAPID = iCAPID;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("GetPayeeProfile is not a cacheable request.");
    }
  }
}
