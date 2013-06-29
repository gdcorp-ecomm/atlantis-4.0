using System;
using System.Runtime.InteropServices;

namespace Atlantis.Framework.DataCacheService
{
  public class GdDataCacheOutOfProcess : IDisposable
  {
    private gdDataCacheLib.IAccess _COMAccessClass;

    private GdDataCacheOutOfProcess()
    {
      _COMAccessClass = new gdDataCacheLib.Access();
    }

    ~GdDataCacheOutOfProcess()
    {
      if (_COMAccessClass != null)
      {
        Marshal.ReleaseComObject(_COMAccessClass);
        _COMAccessClass = null;
      }
      GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
      if (_COMAccessClass != null)
      {
        Marshal.ReleaseComObject(_COMAccessClass);
        _COMAccessClass = null;
      }
    }

    public static GdDataCacheOutOfProcess CreateDisposable()
    {
      return new GdDataCacheOutOfProcess();
    }

    public void ClearCachedData(string cacheName)
    {
      _COMAccessClass.ClearCachedData(cacheName);
    }

    public string DisplayCachedData(int depth, string cacheName)
    {
      return _COMAccessClass.DisplayCachedData(depth, cacheName);
    }

    public string GetAppSetting(string settingName)
    {
      return _COMAccessClass.GetAppSetting(settingName);
    }

    public string GetCacheData(string requestXml)
    {
      return _COMAccessClass.GetCacheData(requestXml);
    }

    public string GetMgrCategoriesForUser(int managerUserId)
    {
      return _COMAccessClass.GetMgrCategoriesForUser(managerUserId);
    }

    public string GetCountriesXml()
    {
      return _COMAccessClass.GetCountryList();
    }

    public string GetStatesXml(int countryId)
    {
      return _COMAccessClass.GetStateList(countryId);
    }

    public string GetCurrencyDataXml()
    {
      return _COMAccessClass.GetCurrencyData("{all}");
    }

    public string GetTLDData(string tld)
    {
      return _COMAccessClass.GetTLDData(tld);
    }

    public string GetTLDList(int privateLabelId, int tldProductType)
    {
      return _COMAccessClass.GetTLDList(privateLabelId, tldProductType);
    }

    public string GetPLData(int privateLabelId, int dataCategoryId)
    {
      return _COMAccessClass.GetPLData(privateLabelId, dataCategoryId);
    }

    public int GetPrivateLabelId(string progId)
    {
      return _COMAccessClass.GetPrivateLabelId(progId);
    }

    public string GetProgId(int privateLabelId)
    {
      return _COMAccessClass.GetProgID(privateLabelId);
    }

    public int GetPrivateLabelType(int privateLabelId)
    {
      return _COMAccessClass.GetPrivateLabelType(privateLabelId);
    }

    public bool IsPrivateLabelActive(int privateLabelId)
    {
      return _COMAccessClass.IsPrivateLabelActive(privateLabelId);
    }

    public int ConvertToPFID(int unifiedProductId, int privateLabelId)
    {
      return _COMAccessClass.GetPFIDByUnifiedID(unifiedProductId, privateLabelId);
    }

    public bool WithOptionsGetListPrice(int unifiedProductId, int privateLabelId, string options, out int price, out bool isEstimate)
    {
      return _COMAccessClass.WithOptionsGetListPriceByUnifiedPFID(privateLabelId, unifiedProductId, options, out price, out isEstimate);
    }

    public bool WithOptionsGetPromoPrice(int unifiedProductId, int privateLabelId, int quantity, string options, out int price, out bool isEstimate)
    {
      return _COMAccessClass.WithOptionsGetPromoPriceByQtyAndUnifiedPFID(privateLabelId, unifiedProductId, quantity, options, out price, out isEstimate);
    }

    public bool WithOptionsIsProductOnSale(int unifiedProductId, int privateLabelId, string options)
    {
      return _COMAccessClass.WithOptionsIsProductOnSaleByUnifiedPFID(privateLabelId, unifiedProductId, options);
    }

    public string GetPriceEstimate(string requestXml)
    {
      return _COMAccessClass.GetPriceEstimateEx(requestXml);
    }

    public string GetShopperRenewingServices(string shopperId)
    {
      return _COMAccessClass.GetShopperRenewingServices(shopperId);
    }
  }
}
