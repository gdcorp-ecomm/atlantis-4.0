using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.FastballCICreate.Interface;
using System.Xml;
using Atlantis.Framework.FastballCICreate.Impl.ClickImpressionServiceAgent;

namespace Atlantis.Framework.FastballCICreate.Impl
{
  public class FastballCICreateRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = new FastballCICreateResponseData();

      ClickImpressionServiceAgent.ClickImpressionService agent = null;
      string response = string.Empty;

      try
      {
        FastballCICreateRequestData requestData = (FastballCICreateRequestData)oRequestData;
        WsConfigElement wsConfig = ((WsConfigElement)oConfig);

        using (agent = new ClickImpressionService())
        {
          agent.Url = wsConfig.WSURL;
          agent.Timeout = (int)oRequestData.RequestTimeout.TotalMilliseconds;

          response = agent.CreateCICodesForItems(requestData.CICodeXML);

          result = new FastballCICreateResponseData(oRequestData, response);

          if (response.IndexOf("success", StringComparison.OrdinalIgnoreCase) >= 0)
          {
            ((FastballCICreateResponseData)result).IsSuccess = true;
          }
        }
      }
      catch (Exception ex)
      {
        result = new FastballCICreateResponseData(oRequestData, ex);
      }
      return result;
    }

    #endregion

  }
}
