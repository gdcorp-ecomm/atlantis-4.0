using System.Globalization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Interface;
using Atlantis.Framework.Providers.Interface.Products;
using System;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Products
{
  public class ProductInfo : IProductInfo
  {
    readonly IProviderContainer _container;
    readonly int _productId;
    readonly Lazy<ISiteContext> _siteContext;
    readonly Lazy<ProductInfoResponseData> _productInfoResponse;
    private readonly Lazy<ProductNamesResponseData> _productNamesResponse;
    private readonly Lazy<int> _nonUnifiedPfid; 

    internal ProductInfo(IProviderContainer container, int productId)
    {
      _container = container;
      _productId = productId;

      _siteContext = new Lazy<ISiteContext>(() => _container.Resolve<ISiteContext>());
      _productInfoResponse = new Lazy<ProductInfoResponseData>(LoadProductInfoData);
      _productNamesResponse = new Lazy<ProductNamesResponseData>(LoadProductNamesForLanguage);
      _nonUnifiedPfid = new Lazy<int>(LoadNonUnifiedPfid);
    }

    private ProductInfoResponseData LoadProductInfoData()
    {
      ProductInfoResponseData result; 
      try
      {
        var request = new ProductInfoRequestData(_productId, _siteContext.Value.PrivateLabelId);
        result = (ProductInfoResponseData)DataCache.DataCache.GetProcessRequest(request, ProductProviderEngineRequests.ProductInfo);
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("LoadProductInfoData", 0, ex.Message + ex.StackTrace, _productId.ToString(CultureInfo.InvariantCulture));
        Engine.Engine.LogAtlantisException(exception);
        result = ProductInfoResponseData.None;
      }
      return result;
    }

    private ProductNamesResponseData LoadProductNamesForLanguage()
    {
      ILocalizationProvider localization;
      if (!_container.TryResolve(out localization))
      {
        return ProductNamesResponseData.Empty;
      }

      if (localization.IsActiveLanguage("en-us"))
      {
        return ProductNamesResponseData.Empty;
      }

      if (_nonUnifiedPfid.Value == 0)
      {
        return ProductNamesResponseData.Empty;
      }

      var request = new ProductNamesRequestData(localization.FullLanguage, _nonUnifiedPfid.Value);

      try
      {
        return (ProductNamesResponseData)DataCache.DataCache.GetProcessRequest(request, ProductProviderEngineRequests.ProductNames);
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("LoadProductNamesForLanguage", 0, ex.Message + ex.StackTrace, request.NonUnifiedPfid + "/" + request.FullLanguage);
        Engine.Engine.LogAtlantisException(exception);
        return ProductNamesResponseData.Empty;
      }
    }

    private int LoadNonUnifiedPfid()
    {
      int result = 0;

      try
      {
        var request = new NonUnifiedPfidRequestData(_productId, _siteContext.Value.PrivateLabelId);
        var response = (NonUnifiedPfidResponseData) DataCache.DataCache.GetProcessRequest(request, ProductProviderEngineRequests.NonUnifiedProductId);
        result = response.NonUnifiedPfid;
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("LoadProductNamesForLanguage", 0, ex.Message + ex.StackTrace, _productId.ToString(CultureInfo.InvariantCulture));
        Engine.Engine.LogAtlantisException(exception);
      }

      return result;
    }

    public string FriendlyDescription
    {
      get
      {
        string result = _productInfoResponse.Value.FriendlyDescription;
        if (!string.IsNullOrEmpty(_productNamesResponse.Value.FriendlyName))
        {
          result = _productNamesResponse.Value.FriendlyName;
        }
        return result;
      }
    }

    public string Name
    {
      get
      {
        string result = _productInfoResponse.Value.Name;
        if (!string.IsNullOrEmpty(_productNamesResponse.Value.Name))
        {
          result = _productNamesResponse.Value.Name;
        }
        return result;
      }
    }

    public int NumberOfPeriods
    {
      get { return _productInfoResponse.Value.NumberOfPeriods; }
    }

    public int ProductTypeId
    {
      get { return _productInfoResponse.Value.ProductTypeId; }
    }

    public RecurringPaymentUnitType RecurringPayment
    {
      get { return _productInfoResponse.Value.RecurringPayment; }
    }
  }
}
