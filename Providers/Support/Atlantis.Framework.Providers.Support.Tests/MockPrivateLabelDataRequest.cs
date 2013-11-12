using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivateLabel.Impl;
using Atlantis.Framework.PrivateLabel.Interface;

namespace Atlantis.Framework.Providers.Support.Tests
{
  public class MockPrivateLabelDataRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = (PrivateLabelDataRequestData) requestData;
      IResponseData response;

      if (request.PrivateLabelId == 1724 && request.DataCategoryId == 44)
      {
         response = PrivateLabelDataResponseData.FromDataValue("1");
      }
      else if (request.PrivateLabelId == 1725 && request.DataCategoryId == 44)
      {
        response = PrivateLabelDataResponseData.FromDataValue("2");
      }
      else if (request.PrivateLabelId == 1828 && request.DataCategoryId == 44)
      {
        response = PrivateLabelDataResponseData.FromDataValue("3");
      }
      else if (request.PrivateLabelId == 1726 && request.DataCategoryId == 44)
      {
        response = PrivateLabelDataResponseData.FromDataValue("1");
      }
      else if (request.PrivateLabelId == 1727 && request.DataCategoryId == 44)
      {
        response = PrivateLabelDataResponseData.FromDataValue("1");
      }
      else if (request.PrivateLabelId == 1726 && request.DataCategoryId == 46)
      {
        response = PrivateLabelDataResponseData.FromDataValue("1111111");
      }
      else if (request.PrivateLabelId == 1727 && request.DataCategoryId == 46)
      {
        response = PrivateLabelDataResponseData.FromDataValue("11111111111");
      }
      else
      {
        PrivateLabelDataRequest handlerRequest = new PrivateLabelDataRequest();
        response = handlerRequest.RequestHandler(requestData, config);
      }

      return response;
    }
  }
}
