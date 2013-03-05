using System;
using System.Runtime.InteropServices;

namespace Atlantis.Framework.DataCacheService
{
  public class GdDataCacheOutOfProcess : IDisposable
  {
    private gdDataCacheLib.AccessClass _COMAccessClass;

    private GdDataCacheOutOfProcess()
    {
      _COMAccessClass = new gdDataCacheLib.AccessClass();
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

    public string GetAppSetting(string settingName)
    {
      return _COMAccessClass.GetAppSetting(settingName);
    }

    public string GetCacheData(string requestXml)
    {
      return _COMAccessClass.GetCacheData(requestXml);
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

    public bool WithOptionsGetListPrice(string nonunifiedPfid, int privateLabelId, string options, out int price, out bool isEstimate)
    {
      object priceObject;
      object isEstimateObject;
      bool result = _COMAccessClass.WithOptionsGetListPrice(privateLabelId, nonunifiedPfid, options, out priceObject, out isEstimateObject);
      price = (int)priceObject;
      isEstimate = (bool)isEstimateObject;
      return result;
    }

    public bool WithOptionsGetPromoPrice(string nonunifiedPfid, int privateLabelId, int quantity, string options, out int price, out bool isEstimate)
    {
      object priceObject;
      object isEstimateObject;
      bool result = _COMAccessClass.WithOptionsGetPromoPriceByQty(privateLabelId, nonunifiedPfid, quantity, options, out priceObject, out isEstimateObject);
      price = (int)priceObject;
      isEstimate = (bool)isEstimateObject;
      return result;
    }

    public bool WithOptionsIsProductOnSale(string nonunifiedPfid, int privateLabelId, string options)
    {
      return _COMAccessClass.WithOptionsIsProductOnSale(privateLabelId, nonunifiedPfid, options);
    }
  }
}
