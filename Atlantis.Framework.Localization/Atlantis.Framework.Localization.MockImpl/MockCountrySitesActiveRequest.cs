using System;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;

namespace Atlantis.Framework.Localization.MockImpl
{
  public class MockCountrySitesActiveRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      if (HttpContext.Current != null)
      {
        var request = requestData as CountrySitesActiveRequestData;
        if (request == null)
        {
          throw new Exception(this.GetType().Name + " requires a request derived from " + typeof(CountrySitesActiveRequestData).Name);
        }
        string strResp = HttpContext.Current.Items[MockLocalizationSettings.CountrySitesActiveTable] as string;
        result = CountrySitesActiveResponseData.FromCacheDataXml(strResp);

      }
      return result;
    }

    #endregion
  }
}
