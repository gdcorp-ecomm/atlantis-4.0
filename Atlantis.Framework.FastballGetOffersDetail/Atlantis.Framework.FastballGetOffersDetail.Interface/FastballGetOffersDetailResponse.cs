using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballGetOffersDetail.Interface
{
  public class FastballGetOffersDetailResponse : IResponseData
  {
    private AtlantisException _exception;

    public bool IsSuccess { get; set; }

    public List<OfferDetail> OfferDetailList { get; set; }
        
    public FastballGetOffersDetailResponse()
    {
    }

    public FastballGetOffersDetailResponse(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(
        requestData, "Atlantis.Framework.FastballGetOffersDetail", ex.Message, ex.StackTrace, ex);
      IsSuccess = false;
    }

    public FastballGetOffersDetailResponse(List<OfferDetail> offerDetailList)
    {
      OfferDetailList = offerDetailList;
    }


    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
