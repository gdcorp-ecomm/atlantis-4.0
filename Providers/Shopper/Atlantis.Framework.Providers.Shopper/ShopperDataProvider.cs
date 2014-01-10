using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Shopper.Interface;
using Atlantis.Framework.Shopper.Interface;
using System;
using System.Collections.Generic;
using System.Web;

namespace Atlantis.Framework.Providers.Shopper
{
  public class ShopperDataProvider : ProviderBase, IShopperDataProvider
  {
    Lazy<IShopperContext> _shopperContext;
    Lazy<ISiteContext> _siteContext;
    Lazy<IProxyContext> _proxyContext;

    private Lazy<GetShopperResponseCache> _getShopperResponseCache =
      new Lazy<GetShopperResponseCache>(() => { return new GetShopperResponseCache(); });
    private Lazy<GetShopperFieldManager> _getShopperFieldManager =
      new Lazy<GetShopperFieldManager>(() => { return new GetShopperFieldManager(); });

    public ShopperDataProvider(IProviderContainer container)
      : base(container)
    {
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _proxyContext = new Lazy<IProxyContext>(() => LoadProxyContext());
    }

    private IProxyContext LoadProxyContext()
    {
      IProxyContext result;
      if (!Container.TryResolve(out result))
      {
        result = null;
      }
      return result;
    }

    public void RegisterNeededFields(params string[] fields)
    {
      if (fields != null)
      {
        _getShopperFieldManager.Value.RegisterNeededFields(fields);
      }
    }

    public void RegisterNeededFields(IEnumerable<string> fields)
    {
      if (fields != null)
      {
        _getShopperFieldManager.Value.RegisterNeededFields(fields);
      }
    }

    public bool TryGetField<T>(string fieldName, out T fieldValue)
    {
      if (string.IsNullOrEmpty(fieldName))
      {
        fieldValue = default(T);
        return false;
      }

      string rawValue;
      if (_getShopperResponseCache.Value.TryGetShopperData(_shopperContext.Value.ShopperId, fieldName, out rawValue))
      {
        return TryConvert(rawValue, out fieldValue);
      }

      try
      {

        _getShopperFieldManager.Value.RegisterNeededField(fieldName);
        var stillNeededFields = _getShopperFieldManager.Value.GetNeededFieldsForShopper(_shopperContext.Value.ShopperId);

        var request = new GetShopperRequestData(_shopperContext.Value.ShopperId, GetOriginIpAddress(), GetRequestedBy(),  stillNeededFields);
        var response = (GetShopperResponseData)Engine.Engine.ProcessRequest(request, ShopperProviderEngineRequests.GetShopper);

        _getShopperFieldManager.Value.RegisterResponseFields(response);
        _getShopperResponseCache.Value.CacheShopperData(response);

        if (response.HasFieldValue(fieldName))
        {
          return TryConvert(response.GetFieldValue(fieldName), out fieldValue);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + ex.StackTrace;
        string data = "shopperid=" + _shopperContext.Value.ShopperId + ":fieldname=" + fieldName;
        var aex = new AtlantisException("ShopperDataProvider.TryGetField", 0, message, data);
        Engine.Engine.LogAtlantisException(aex);
      }

      fieldValue = default(T);
      return false;
    }

    private bool TryConvert<T>(string rawValue, out T convertedValue)
    {
      convertedValue = default(T);

      try
      {
        convertedValue = (T)Convert.ChangeType(rawValue, typeof(T));
        return true;
      }
      catch
      {
        return false;
      }
    }

    private Lazy<ShopperSpecificSessionDataItem<bool>> _shopperIsValidData =
      new Lazy<ShopperSpecificSessionDataItem<bool>>(() => { return new ShopperSpecificSessionDataItem<bool>("ShopperDataProvider.IsShopperValid"); });

    public bool IsShopperValid()
    {
      if (string.IsNullOrEmpty(_shopperContext.Value.ShopperId))
      {
        return false;
      }

      bool result;
      if (_shopperIsValidData.Value.TryGetData(_shopperContext.Value.ShopperId, out result))
      {
        return result;
      }

      try
      {
        var request = new VerifyShopperRequestData(_shopperContext.Value.ShopperId);
        var response = (VerifyShopperResponseData)Engine.Engine.ProcessRequest(request, ShopperProviderEngineRequests.VerifyShopper);
        result = response.IsVerified && (response.PrivateLabelId == _siteContext.Value.PrivateLabelId);
        _shopperIsValidData.Value.SetData(_shopperContext.Value.ShopperId, result);
      }
      catch(Exception ex)
      {
        string message = ex.Message + ex.StackTrace;
        string data = _shopperContext.Value.ShopperId + ":" + _siteContext.Value.PrivateLabelId;
        var exception = new AtlantisException("ShopperDataProvider.IsShopperValue", 0, message, data);
        Engine.Engine.LogAtlantisException(exception);
        result = true; // on an exception we cannot verify, so we allow the calling code to continue
      }

      return result;
    }

    public bool TryCreateNewShopper()
    {
      bool result = false;

      try
      {
        var request = new CreateShopperRequestData(_siteContext.Value.PrivateLabelId, GetOriginIpAddress(), GetRequestedBy());
        var response = (CreateShopperResponseData)Engine.Engine.ProcessRequest(request, ShopperProviderEngineRequests.CreateShopper);
        if (response.Status.Status == ShopperResponseStatusType.Success)
        {
          _shopperContext.Value.SetNewShopper(response.ShopperId);
          result = true;
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + ex.StackTrace;
        var exception = new AtlantisException("ShopperDataProvider.TryCreateShopper", 0, message, string.Empty);
        Engine.Engine.LogAtlantisException(exception);
      }

      return result;
    }

    public bool TryUpdateShopper(IDictionary<string, string> updates)
    {
      bool result = false;

      if (!string.IsNullOrEmpty(_shopperContext.Value.ShopperId))
      {
        try
        {
          var request = new UpdateShopperRequestData(_shopperContext.Value.ShopperId, GetOriginIpAddress(), GetRequestedBy(), updates);
          var response = (UpdateShopperResponseData)Engine.Engine.ProcessRequest(request, ShopperProviderEngineRequests.UpdateShopper);
          if (response.Status.Status == ShopperResponseStatusType.Success)
          {
            result = true;
            _getShopperResponseCache.Value.ClearShopperData(_shopperContext.Value.ShopperId);
            _getShopperFieldManager.Value.ClearShopper(_shopperContext.Value.ShopperId);
          }
        }
        catch (Exception ex)
        {
          string message = ex.Message + ex.StackTrace;
          var exception = new AtlantisException("ShopperDataProvider.TryUpdateShopper", 0, message, string.Empty);
          Engine.Engine.LogAtlantisException(exception);
        }
      }

      return result;
    }

    public ShopperUpdateResultType UpdateShopperInfo(IDictionary<string, string> updates)
    {
      var resultCode = ShopperUpdateResultType.UnknownError;

      if (!string.IsNullOrEmpty(_shopperContext.Value.ShopperId))
      {
        try
        {
          var request = new UpdateShopperRequestData(_shopperContext.Value.ShopperId, GetOriginIpAddress(), GetRequestedBy(), updates);
          var response = (UpdateShopperResponseData)Engine.Engine.ProcessRequest(request, ShopperProviderEngineRequests.UpdateShopper);
          if (response.Status.Status == ShopperResponseStatusType.Success)
          {
            resultCode = ShopperUpdateResultType.Success;
            _getShopperResponseCache.Value.ClearShopperData(_shopperContext.Value.ShopperId);
            _getShopperFieldManager.Value.ClearShopper(_shopperContext.Value.ShopperId);
          }
          else
          {
            resultCode = ShopperUpdateErrorMapper.GetUpdateResultType(response.Status.ErrorCode);
          }
        }
        catch (Exception ex)
        {
          string message = ex.Message + ex.StackTrace;
          var exception = new AtlantisException("ShopperDataProvider.UpdateShoperInfo", 0, message, string.Empty);
          Engine.Engine.LogAtlantisException(exception);
        }
      }

      return resultCode;
    }

    private string GetOriginIpAddress()
    {
      if (_proxyContext.Value != null)
      {
        return _proxyContext.Value.OriginIP;
      }

      if (HttpContext.Current != null)
      {
        return HttpContext.Current.Request.UserHostAddress;
      }

      return Environment.MachineName;
    }

    private string GetRequestedBy()
    {
      return Environment.MachineName + ".ShopperDataProvider";
    }
  }
}
