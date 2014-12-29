using System;

using Atlantis.Framework.AddItem.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddItem.Impl
{
  public class AddItemRequest : IRequest
  {
    public static string version = string.Empty;

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      AddItemResponseData responseData;
      string responseXml = string.Empty;

      try
      {
        var addItemRequestData = (AddItemRequestData)oRequestData;
        using (var basketWs = new WscgdBasketServiceWithAuthToken(addItemRequestData.AuthToken))
        {
          basketWs.Url = ((WsConfigElement) oConfig).WSURL;
          basketWs.Timeout = (int) addItemRequestData.RequestTimeout.TotalMilliseconds;

          responseXml = basketWs.AddItem(addItemRequestData.ShopperID, addItemRequestData.ToXML());
        }

        if (responseXml.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
        {
          var exAtlantis = new AtlantisException(oRequestData,
                                                 "AddItemRequest.RequestHandler",
                                                 responseXml,
                                                 oRequestData.ToXML());

          responseData = new AddItemResponseData(responseXml, exAtlantis);
        }
        else
        {
          responseData = new AddItemResponseData(responseXml);
        }
      }
      catch(AtlantisException exAtlantis)
      {
        responseData = new AddItemResponseData(responseXml, exAtlantis);
      }
      catch(Exception ex)
      {
        responseData = new AddItemResponseData(responseXml, oRequestData, ex);
      }

      return responseData;
    }

    #endregion
  }
}
