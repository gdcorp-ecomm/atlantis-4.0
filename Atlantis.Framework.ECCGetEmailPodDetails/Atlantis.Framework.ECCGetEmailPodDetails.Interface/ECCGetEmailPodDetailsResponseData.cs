using System;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetEmailPodDetails.Interface
{
  public class ECCGetEmailPodDetailsResponseData : EccResponseDataBase<EccEmailPodDetails>
  {
    public EccEmailPodDetails EmailPodDetails
    {
      get
      {
        EccEmailPodDetails eccEmailPodDetails = null;
        if (Response != null && Response.Item != null && Response.Item.Results != null && Response.Item.Results.Count > 0)
        {
          eccEmailPodDetails = Response.Item.Results[0];
        }

        return eccEmailPodDetails;
      }
    }

    public ECCGetEmailPodDetailsResponseData(string resultJson) : base(resultJson)
    {
    }

	  public ECCGetEmailPodDetailsResponseData(RequestData requestData, Exception exception) : base(requestData, exception)
	  {
	  }
  }
}
