using System;
using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Basket.Impl
{
  /// <summary>
  /// The BasketDeleteRequest class handles basket delete requests.
  /// </summary>
  public class BasketDeleteRequest : IRequest
  {
    private const int LOCKCUSTOMER = 1;
    private const int LOCKMANAGER = 2;

    private readonly IBasketService _basketService;

    /// <summary>
    /// Initializes a new instance of the BasketDeleteRequest class using the basket service
    /// specified. If no service is specified, BasketServiceProxy will be used.
    /// </summary>
    /// <param name="service">Optional parameter that specifies a service to use for
    /// deletion. The default value is null.</param>
    public BasketDeleteRequest(IBasketService service = null)
    {
      _basketService = service ?? new BasketServiceProxy();
    }

    /// <summary>
    /// Handles the delete request using request data and config specified.
    /// </summary>
    /// <param name="requestData">Request data for deletion.</param>
    /// <param name="config">Configuration</param>
    /// <returns>An object that implements IResponseData, which has response data.</returns>
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var deleteRequestData = requestData as BasketDeleteRequestData;
      if (deleteRequestData == null)
      {
        throw new ArgumentException("requestData is not of type BasketDeleteRequestData");
      }

      var itemKeysToDelete = deleteRequestData.GetItemKeysToDelete();
      if (string.IsNullOrWhiteSpace(itemKeysToDelete))
      {
        throw new ArgumentException(
          "requestData.GetItemKeysToDelete() returned empty string. Items to be deleted is empty.");
      }

      var wsConfig = config as WsConfigElement;
      if (wsConfig == null)
      {
        throw new ArgumentException("config is not of type WsConfigElement");
      }

      int lockingMode = deleteRequestData.IsManager ? LOCKMANAGER : LOCKCUSTOMER;
      string responseXml = _basketService.DeleteItemsByPairWithType(deleteRequestData.ShopperID,
        deleteRequestData.BasketType,
        itemKeysToDelete, lockingMode, wsConfig.WSURL,
        (int) deleteRequestData.RequestTimeout.TotalMilliseconds);

      if (responseXml.Contains("<error>"))
      {
        throw new AtlantisException(deleteRequestData, "BasketDeleteRequest.RequestHandler", responseXml,
          deleteRequestData.ToXML());
      }
      
      return BasketDeleteResponseData.FromData(responseXml);
    }
  }
}
