using System;
using System.Collections.Generic;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Links.Interface;

namespace Atlantis.Framework.Links.MockImpl
{
  public class MockLinkInfoRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IDictionary<string, ILinkInfo> data = null;
      IDictionary<string, string> baseUrlData = null;
      if (HttpContext.Current != null)
      {
        var request = oRequestData as LinkInfoRequestData;
        if ( request == null )
        {
          throw new Exception(this.GetType().Name + " requires a request derived from " + typeof(LinkInfoRequestData).Name);
        }
        data = HttpContext.Current.Items[MockLinkInfoRequestContextSettings.LinkInfoTable + "." + request.ContextID] as IDictionary<string, ILinkInfo>;
        baseUrlData = HttpContext.Current.Items[MockLinkInfoRequestContextSettings.LinkInfoByBaseUrlTable + "." + request.ContextID] as IDictionary<string, string>;
      }
      data = data ?? new Dictionary<string, ILinkInfo>();
      baseUrlData = baseUrlData ?? new Dictionary<string, string>();
      return new LinkInfoResponseData( data, baseUrlData );
    }

    #endregion

  }
}
