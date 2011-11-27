using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.OrionAddAttribute.Impl.OrionAccountOperations;
using Atlantis.Framework.OrionAddAttribute.Interface;
using Atlantis.Framework.OrionSecurityAuth.Interface;

namespace Atlantis.Framework.OrionAddAttribute.Impl
{
  public class OrionAddAttributeRequest : IRequest
  {
    private const string _systemNamespace = "GoDaddy";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      int responseCode = -1;
      var request = requestData as OrionAddAttributeRequestData;
      OrionAddAttributeResponseData response;

      try
      {
        string error = string.Empty;
        string attributeUid = string.Empty;
        OrionSecurityAuthResponseData responseSecurityData = GetOrionAuthToken(requestData);
        if (responseSecurityData.IsSuccess && !string.IsNullOrEmpty(responseSecurityData.AuthToken))
        {
          AddAttributeRequest addAttributeRequest = BuildAddAttributeRequest(request);
          OperationResponse[] addAttributeResponse;
          string[] errors;

          using (var accountOperationsWs = new AccountOperations())
          {
            accountOperationsWs.Url = ((WsConfigElement) config).WSURL;
            accountOperationsWs.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
            accountOperationsWs.SecureHeaderValue = new SecureHeader { Token = responseSecurityData.AuthToken };

            int returnCode = accountOperationsWs.AddAttribute(new AddAttributeRequest[1] {addAttributeRequest}, out addAttributeResponse, out errors);

            responseCode = addAttributeResponse == null || addAttributeResponse.Length == 0
                             ? returnCode
                             : addAttributeResponse[0].Result;
            attributeUid = addAttributeResponse == null || addAttributeResponse.Length == 0
                             ? string.Empty
                             : addAttributeResponse[0].items[0].ItemValue;
            error = errors == null || errors.Length < 1 ? string.Empty : errors[0];
          }
        }

        response = new OrionAddAttributeResponseData(responseCode, attributeUid, error);
      }
      catch (AtlantisException atlEx)
      {
        response = new OrionAddAttributeResponseData(atlEx);
      }
      catch (Exception ex)
      {
        string data = string.Format("AccountUID:{0}, AttributeName:{1}", request.OrionResourceId, request.Attribute.Name);
        var atlEx = new AtlantisException(requestData, "OrionAddAttribute.Impl.RequestHandler", ex.Message, data);
        response = new OrionAddAttributeResponseData(atlEx);
      }

      return response;
    }

    private static AddAttributeRequest BuildAddAttributeRequest(OrionAddAttributeRequestData request)
    {
      var attributeRequest = new AddAttributeRequest();
      attributeRequest.RequestedBy = request.RequestedBy;
      attributeRequest.RequestedByLogin = request.RequestedBy;
      attributeRequest.RequestIdx = 0;

      attributeRequest.Identifier = new AccountIdentifier();
      attributeRequest.Identifier.AccountUid = request.OrionResourceId;
      attributeRequest.Identifier.CustomerNum = request.ShopperID;
      attributeRequest.Identifier.ResellerId = request.PrivateLabelId.ToString();
      attributeRequest.Identifier.SystemNamespace = _systemNamespace;

      attributeRequest.Attr = new AccountAttribute();
      attributeRequest.Attr.Name = request.Attribute.Name;
      attributeRequest.Attr.Status = "active";
      attributeRequest.Attr.CanBeModified = 1;
      attributeRequest.Attr.IsTemplateAttribute = 0;
      var elementsList = new List<AccountElement>(request.Attribute.Elements.Count);
      request.Attribute.Elements.ForEach(e => elementsList.Add(new AccountElement { Name = e.Key, Value = e.Value }));
      attributeRequest.Attr.AccountElements = elementsList.ToArray();

      return attributeRequest;
    }

    private static OrionSecurityAuthResponseData GetOrionAuthToken(RequestData requestData)
    {
      var securityRequestData = new OrionSecurityAuthRequestData(requestData.ShopperID,
                                                                 requestData.SourceURL,
                                                                 requestData.OrderID,
                                                                 requestData.Pathway,
                                                                 requestData.PageCount,
                                                                 "Atlantis.Framework.OrionAddAttribute");

      var responseSecurityData = (OrionSecurityAuthResponseData)DataCache.DataCache.GetProcessRequest(securityRequestData, securityRequestData.OrionSecurityAuthRequestType);

      return responseSecurityData;
    }
  }
}
