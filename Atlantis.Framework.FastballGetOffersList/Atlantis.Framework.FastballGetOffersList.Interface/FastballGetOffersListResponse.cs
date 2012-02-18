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

    public FastballGetOffersListResponse(LocationResult locationResult)
    {
      IsSuccess = true;
      LocationResult = locationResult;
    }

    public FastballGetOffersListResponse(RequestData requestData, Exception ex)
    {
      IsSuccess = false;
      _exception = new AtlantisException(requestData, requestData.GetType().ToString(), ex.Message, ex.StackTrace, ex);
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
