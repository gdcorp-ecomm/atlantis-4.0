using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atlantis.Framework.DomainsRAA.Interface.DomainsRAAStatus;
using Atlantis.Framework.DomainsRAA.Interface.DomainsRAAVerify;
using Atlantis.Framework.Interface;
using System;
using Atlantis.Framework.Providers.DomainsRAA.Interface;
using Atlantis.Framework.Providers.DomainsRAA.Interface.Items;

using DomainsRAAService= Atlantis.Framework.DomainsRAA.Interface;

namespace Atlantis.Framework.Providers.DomainsRAA
{
  public class DomainsRAAProvider : ProviderBase, IDomainsRAAProvider // Framework providers should implement a corresponding interface
  {
    private readonly Lazy<IShopperContext> _shopperContext;

    public DomainsRAAProvider(IProviderContainer container)
      : base(container)
    {
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
    }


    public bool TryQueueVerification(IVerifyRequestItems verificationItems, out IEnumerable<DomainsRAAErrorCodes> errorCodes)
    {
      var isSuccess = false;
      errorCodes = new List<DomainsRAAErrorCodes>(0);
      try
      {

       var requestItemTypes = new List<DomainsRAAService.Items.VerifyRequestItem>(verificationItems.Items.Count());

        foreach (var item in verificationItems.Items)
        {
          requestItemTypes.Add(DomainsRAAService.Items.VerifyRequestItem.Create(item.ItemType, item.ItemTypeValue));
        }

        DomainsRAAService.DomainsRAAReasonCodes reasonCode;
        if (!Enum.TryParse(verificationItems.ReasonCode.ToString(), out reasonCode))
        {
          reasonCode = DomainsRAAService.DomainsRAAReasonCodes.None;
        }

        var domainsRAAVerifyItems = DomainsRAAService.Items.VerifyRequestItems.Create(
          verificationItems.RegistrationType,
          HttpContext.Current.Request.UserHostAddress,
          requestItemTypes,
          reasonCode,
          verificationItems.DomainId
          );

        var request = new DomainsRAAVerifyRequestData(_shopperContext.Value.ShopperId, domainsRAAVerifyItems);

        var response = (DomainsRAAVerifyResponseData)Engine.Engine.ProcessRequest(request, 765);

        if (response != null)
        {
          isSuccess = response.IsSuccess;

          if (!isSuccess && response.HasErrorCodes)
          {
            var codes = new List<DomainsRAAErrorCodes>(response.ErrorCodes.Count());

            foreach (var code in response.ErrorCodes)
            {
              DomainsRAAErrorCodes errorCode;
              Enum.TryParse(code.ToString(), out errorCode);

              codes.Add(errorCode);
            }

            errorCodes = codes;
          }
        }
      }
      catch (Exception ex)
      {
        isSuccess = false;
        var aex = new AtlantisException("DomainsRAAProvider.TryQueueVerification", "0", ex.StackTrace, ex.ToString(), null, null);
        Engine.Engine.LogAtlantisException(aex); 
      }

      if (!isSuccess && !errorCodes.Any())
      {
        errorCodes = new List<DomainsRAAErrorCodes>(1) { DomainsRAAErrorCodes.Exception };
      }

      return isSuccess;
    }
    

    public bool TryGetStatus(IVerifyRequestItems requestItems, out IDomainsRAAStatus raaStatus)
    {
      raaStatus = null;

      try
      {
        var serviceRequestTypes = new List<DomainsRAAService.Items.VerifyRequestItem>(requestItems.Items.Count());


        foreach (var verifyRequestItem in requestItems.Items)
        {
          var itemType = DomainsRAAService.Items.VerifyRequestItem.Create(verifyRequestItem.ItemType, verifyRequestItem.ItemTypeValue);
          serviceRequestTypes.Add(itemType);
        }

        DomainsRAAService.DomainsRAAReasonCodes reasonCode;
        Enum.TryParse(requestItems.ReasonCode.ToString(), out reasonCode);

        var serviceRequestItems = DomainsRAAService.Items.VerifyRequestItems.Create(requestItems.RegistrationType,
          HttpContext.Current.Request.UserHostAddress,
          serviceRequestTypes,
          reasonCode,
          requestItems.DomainId);

        var request = new DomainsRAAStatusRequestData(serviceRequestItems);

        var response = Engine.Engine.ProcessRequest(request, 767) as DomainsRAAStatusResponseData;

        if (response != null)
        {
          var verifiedResponseItems = new List<IVerifiedResponseItem>(response.VerifiedResponseItems.Count());
          if (response.HasVerifiedResponseCodes)
          {
            foreach (var responseItem in response.VerifiedResponseItems)
            {
               DomainsRAAVerifyCode verifyCode;
              if (!Enum.TryParse(responseItem.ItemVerifiedCode.ToString(), out verifyCode))
              {
                verifyCode = DomainsRAAVerifyCode.None;
              }

              var verifiedStatusItem = VerifiedResponseItem.Create(responseItem.ItemType, responseItem.ItemTypeValue, verifyCode, responseItem.ItemValidationGuid);

              verifiedResponseItems.Add(verifiedStatusItem);
            }
          }

          var errorCodes = new List<DomainsRAAErrorCodes>(response.ErrorCodes.Count());
          if (response.HasErrorCodes)
          {

            foreach (var code in response.ErrorCodes)
            {
              DomainsRAAErrorCodes errorCode;
              Enum.TryParse(code.ToString(), out errorCode);

              errorCodes.Add(errorCode);
            }
          }

          raaStatus = DomainsRAAStatus.Create(verifiedResponseItems, errorCodes);
        }
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException("DomainsRAAProvider.TryGetStatus", "0", ex.StackTrace, ex.ToString(), null, null);
        Engine.Engine.LogAtlantisException(aex); 
      }

      if (raaStatus == null)
      {
        var errorCodes = new List<DomainsRAAErrorCodes>(1) {DomainsRAAErrorCodes.Exception};
        raaStatus = DomainsRAAStatus.Create(null, errorCodes);
      }

      return !raaStatus.HasErrorCodes;
    }
  }
}
