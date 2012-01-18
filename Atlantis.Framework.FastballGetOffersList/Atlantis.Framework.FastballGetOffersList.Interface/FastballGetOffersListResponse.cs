using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballGetOffersList.Interface
{
  public class FastballGetOffersListResponse : IResponseData
  {
    private AtlantisException _exception;
    
    public bool IsSuccess{ get; set; }
    
    public LocationResult LocationResult { get; set; }

    public FastballGetOffersListResponse()
    {
    }

    public FastballGetOffersListResponse(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(
        requestData, "Atlantis.Framework.FastballGetOffersList", ex.Message, ex.StackTrace, ex);
      IsSuccess = false;
    }

    public FastballGetOffersListResponse(LocationResult locationResult)
    {
      LocationResult = locationResult;
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

  }
}
