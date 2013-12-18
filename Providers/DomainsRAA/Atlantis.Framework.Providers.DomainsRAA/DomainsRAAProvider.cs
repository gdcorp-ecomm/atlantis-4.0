using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atlantis.Framework.DomainsRAA.Interface.DomainsRAASetVerified;
using Atlantis.Framework.DomainsRAA.Interface.DomainsRAAStatus;
using Atlantis.Framework.DomainsRAA.Interface.DomainsRAAQueueVerify;
using Atlantis.Framework.Interface;
using System;
using Atlantis.Framework.Providers.DomainsRAA.Interface;
using Atlantis.Framework.Providers.DomainsRAA.Interface.VerificationItems;
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
    
    private DomainsRAAVerifyCode GetVerifyCode(DomainsRAAService.DomainsRAAVerifyCode serviceVerifyCode)
    {
      DomainsRAAVerifyCode verifyCode;

      switch (serviceVerifyCode)
      {
        case DomainsRAAService.DomainsRAAVerifyCode.ShopperVerified:
        case DomainsRAAService.DomainsRAAVerifyCode.DomainRecordVerified:
        case DomainsRAAService.DomainsRAAVerifyCode.ShopperArtifactVerified:
          verifyCode = DomainsRAAVerifyCode.Verified;
          break;
        case DomainsRAAService.DomainsRAAVerifyCode.ShopperVerifyPending:
        case DomainsRAAService.DomainsRAAVerifyCode.ShopperArtifactVerifyPending:
        case DomainsRAAService.DomainsRAAVerifyCode.DomainRecordPendingManualVerify:
          verifyCode = DomainsRAAVerifyCode.VerifyPending;
          break;
        default:
          verifyCode = DomainsRAAVerifyCode.NotVerified;
          break;
      }

      return verifyCode;
    }

    public bool TryQueueVerification(IVerification verification, out IEnumerable<Errors> errorCodes)
    {
      var isSuccess = false;
      errorCodes = new List<Errors>(0);
      try
      {

        var requestItemTypes = new List<DomainsRAAService.Items.ItemElement>(verification.VerifyItems.Items.Count);

        foreach (var item in verification.VerifyItems.Items)
        {
          requestItemTypes.Add(DomainsRAAService.Items.ItemElement.Create(item.ItemType, item.ItemTypeValue));
        }

        DomainsRAAService.DomainsRAAReasonCodes reasonCode;
        if (!Enum.TryParse(verification.ReasonCode.ToString(), out reasonCode))
        {
          throw new Exception("DomainsRAAReasonCodes expected");
        }

        var domainsRAAVerifyItems = DomainsRAAService.Items.VerificationItemsElement.Create(
          verification.VerifyItems.RegistrationType,
          requestItemTypes,
          verification.VerifyItems.DomainId
          );

        var verificationItem = DomainsRAAService.Items.VerificationItemElement.Create(_shopperContext.Value.ShopperId,
          HttpContext.Current.Request.UserHostAddress,
          domainsRAAVerifyItems,
          reasonCode);

        var request = new DomainsRAAQueueVerifyRequestData(verificationItem);

        var response = (DomainsRAAQueueVerifyResponseData)Engine.Engine.ProcessRequest(request, 765);

        if (response != null)
        {
          isSuccess = response.IsSuccess;

          if (!isSuccess && response.HasErrorCodes)
          {
            var codes = new List<Errors>(response.ErrorCodes.Count());

            foreach (var code in response.ErrorCodes)
            {
              Errors errorCode;
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
        errorCodes = new List<Errors>(1) { Errors.Exception };
      }

      return isSuccess;
    }
    
    public bool TryGetStatus(IList<IItem> requestItems, out IDomainsRAAStatus raaStatus)
    {
      raaStatus = null;

      try
      {
        var serviceRequestTypes = new List<DomainsRAAService.Items.ItemElement>(requestItems.Count());

        foreach (var verifyRequestItem in requestItems)
        {
          var itemType = DomainsRAAService.Items.ItemElement.Create(verifyRequestItem.ItemType, verifyRequestItem.ItemTypeValue);
          serviceRequestTypes.Add(itemType);
        }


        var request = new DomainsRAAStatusRequestData(HttpContext.Current.Request.UserHostAddress, serviceRequestTypes);

        var response = Engine.Engine.ProcessRequest(request, 767) as DomainsRAAStatusResponseData;

        if (response != null)
        {
          var verifiedResponseItems = new List<IVerifiedResponseItem>(response.VerifiedResponseItems.Count());
          if (response.HasVerifiedResponseCodes)
          {
            foreach (var responseItem in response.VerifiedResponseItems)
            {
              var verifyCode = GetVerifyCode(responseItem.ItemVerifiedCode);

              var verifiedStatusItem = VerifiedResponseItem.Create(responseItem.ItemType, responseItem.ItemTypeValue, verifyCode, responseItem.ItemValidationGuid);

              verifiedResponseItems.Add(verifiedStatusItem);
            }
          }

          var errorCodes = new List<Errors>(response.ErrorCodes.Count());
          if (response.HasErrorCodes)
          {

            foreach (var code in response.ErrorCodes)
            {
              Errors errorCode;
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
        var errorCodes = new List<Errors>(1) {Errors.Exception};
        raaStatus = DomainsRAAStatus.Create(null, errorCodes);
      }

      return !raaStatus.HasErrorCodes;
    }

    public bool TrySetVerifiedToken(IVerification verification, out IEnumerable<Errors> errorCodes)
    {
      var isSuccess = false;
      errorCodes = new List<Errors>(0);
      try
      {

        var requestItemTypes = new List<DomainsRAAService.Items.ItemElement>(verification.VerifyItems.Items.Count());

        foreach (var item in verification.VerifyItems.Items)
        {
          requestItemTypes.Add(DomainsRAAService.Items.ItemElement.Create(item.ItemType, item.ItemTypeValue));
        }

        DomainsRAAService.DomainsRAAReasonCodes reasonCode;
        if (!Enum.TryParse(verification.ReasonCode.ToString(), out reasonCode))
        {
          throw new Exception("DomainsRAAReasonCodes expected");
        }

        var domainsRAAVerifyItems = DomainsRAAService.Items.VerificationItemsElement.Create(
          verification.VerifyItems.RegistrationType,
          requestItemTypes,
          verification.VerifyItems.DomainId,
          verification.VerifyItems.VerfiedIp
          );

        var verificationItem = DomainsRAAService.Items.VerificationItemElement.Create(_shopperContext.Value.ShopperId,
          HttpContext.Current.Request.UserHostAddress,
          domainsRAAVerifyItems,
          reasonCode);

        var request = new DomainsRAASetVerifiedRequestData(verificationItem);

        var response = (DomainsRAASetVerifiedResponseData)Engine.Engine.ProcessRequest(request, 783);

        if (response != null)
        {
          isSuccess = response.IsSuccess;

          if (!isSuccess && response.HasErrorCodes)
          {
            var codes = new List<Errors>(response.ErrorCodes.Count());

            foreach (var code in response.ErrorCodes)
            {
              Errors errorCode;
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
        var aex = new AtlantisException("DomainsRAAProvider.TrySetVerfiedToken", "0", ex.StackTrace, ex.ToString(), null, null);
        Engine.Engine.LogAtlantisException(aex);
      }

      if (!isSuccess && !errorCodes.Any())
      {
        errorCodes = new List<Errors>(1) { Errors.Exception };
      }

      return isSuccess;
    }
  }
}
