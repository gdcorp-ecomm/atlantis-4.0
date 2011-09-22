using System;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetCalendarDetails.Interface
{
  public class ECCGetCalendarDetailsResponseData : EccResponseDataBase<EccCalendarDetails>
  {
    public ECCGetCalendarDetailsResponseData(string jsonResponse) : base(jsonResponse)
    {
      
    }
    
    public ECCGetCalendarDetailsResponseData(RequestData requestData, Exception ex) : base(requestData,ex)
    {
    }

  }
}
