using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DataCache
{
  public static class DataCache
  {
    const int MCAST_PORT = 7813;
    const string MCAST_ADDR = "224.7.8.13";
    static CacheManager _cacheManger;

    static DataCache()
    {
      try
      {
        _cacheManger = new CacheManager();

        Thread thread = new Thread(new ThreadStart(ListenThreadProc));
        thread.IsBackground = true;
        thread.Start();
      }
      catch (ThreadAbortException)
      { }
      catch (Exception ex)
      {
        try
        {
          LogError("DataCache (Type Initializer)", string.Empty, ex);
        }
        catch { }
      }
    }

    private static void ProcessRequest(string request)
    {
      HashSet<int> privateLabelIds = new HashSet<int>();

      try
      {
        int index = request.IndexOf('/');

        if (index > -1)
        {
          string sPLIDs = request.Substring(index + 1);
          request = request.Substring(0, index);

          foreach (string sPLID in sPLIDs.Split(','))
            privateLabelIds.Add(int.Parse(sPLID));
        }

        if (String.Compare(request, "all", true) == 0)
        {
          _cacheManger.ClearCacheAllCachesByPLID(privateLabelIds);
        }
        else if (String.Compare(request, "genericXML", true) == 0)
        {
          _cacheManger.ReloadGenericCaches();
        }
        else
        {
          _cacheManger.ClearCacheDataByPLID(request, privateLabelIds);
        }
      }
      catch (Exception ex)
      {
        LogError("ProcessRequest", request, ex);
      }
    }

    private static void ListenThreadProc()
    {
      try
      {
        IPAddress mcastAddress = IPAddress.Parse(MCAST_ADDR);
        IPAddress localAddress = IPAddress.Loopback;
        Socket socket = null;

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        socket.Bind(new IPEndPoint(localAddress, MCAST_PORT));

        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
                               new MulticastOption(mcastAddress, localAddress));

        string sRequest = string.Empty;
        byte[] bytes = new byte[512];
        while (true)
        {
          try
          {
            int iNumBytes = socket.Receive(bytes);
            sRequest = ASCIIEncoding.ASCII.GetString(bytes, 0, iNumBytes);

            if (iNumBytes > 0 && sRequest[iNumBytes - 1] == '*')
              ProcessRequest(sRequest.Substring(0, iNumBytes - 1));
          }
          catch (ThreadAbortException)
          { }
          catch (Exception ex)
          {
            LogError("ListenThreadProc: In request loop", sRequest, ex);
          }
        }
      }
      catch (ThreadAbortException)
      { }
      catch (Exception ex)
      {
        try
        {
          LogError("ListenThreadProc: Thread failed to run", string.Empty, ex);
        }
        catch { }
      }
    }

    public static void ClearCachedData(string cacheName)
    {
      using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
      {
        oCacheWrapper.COMAccessClass.ClearCachedData(cacheName);
      }
    }

    public static void ClearInProcessCachedData(string cacheName)
    {
      _cacheManger.ClearCacheData(cacheName);
    }

    public static string DisplayInProcessCachedData(string cacheName)
    {
      string result = string.Empty;
      try
      {
        result = _cacheManger.DisplayCache(cacheName);
      }
      catch (Exception ex)
      {
        LogError("DisplayInProcessCachedData:" + cacheName, cacheName, ex);
        throw ex;
      }

      return result;
    }

    public static string DisplayOutProcessCachedData(int depth, string cacheName)
    {
      string result = string.Empty;
      try
      {
        using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
        {
          result = oCacheWrapper.COMAccessClass.DisplayCachedData(depth, cacheName);
        }
      }
      catch (Exception ex)
      {
        LogError("DisplayOutProcessCachedData:" + cacheName, depth.ToString(), ex);
        throw ex;
      }

      return result;
    }

    public static string GetStats()
    {
      return _cacheManger.GetStats();
    }

    private static void LogError(string cacheName, string key, Exception ex)
    {
      if (ex.GetType() != typeof(ThreadAbortException))
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        string source = cacheName + ":" + key;
        AtlantisException aex = new AtlantisException(source, "0", message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }
    }

    public static DataTable GetAdServerCategories(string appName, string pageName, string locationName)
    {
      DataTable dtResult = null;
      string sCacheName = "GetAdServerCategories";
      string sKey = appName + "-" + pageName + "-" + locationName;
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          ADODB.Recordset oRecordSet = null;
          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            oRecordSet = (ADODB.Recordset)oCacheWrapper.COMAccessClass.GetAdServerCategories(appName, pageName, locationName);
          }

          dtResult = GetDataTableFromRecordset(oRecordSet);

          oCache.AddValue(sKey, dtResult, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            dtResult = (DataTable)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        dtResult = (DataTable)oValue.Value;

      return dtResult;
    }

    public static string GetAppSetting(string settingName)
    {
      string sCacheName = "GetAppSetting";
      string sSettingValue = string.Empty;
      string sKey = settingName;
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sSettingValue = oCacheWrapper.COMAccessClass.GetAppSetting(settingName);
          }

          oCache.AddValue(sKey, sSettingValue, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            sSettingValue = (string)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        sSettingValue = (string)oValue.Value;

      return sSettingValue;
    }

    public delegate T GetCustomDataDelegate<T>(string requestKey);

    public static T GetCustomCacheData<T>(string request, GetCustomDataDelegate<T> getCustomDataForCache)
    {
      T result = default(T);
      string cacheName = "CustomCacheData." + typeof(T).FullName;
      Cache cache = _cacheManger.GetCache(cacheName, false);
      CachedValue cachedValue = null;
      bool isValid = cache.TryGetValue(request, out cachedValue);

      if (isValid)
      {
        result = (T)cachedValue.Value;
      }
      else
      {
        try
        {
          if (cachedValue != null)
          {
            cachedValue.MarkInProgress();
          }

          result = getCustomDataForCache(request);

          cache.AddValue(request, result, 0, cachedValue);
        }
        catch (Exception ex)
        {
          if (cachedValue != null)
          {
            result = (T)cachedValue.Value;
            cache.RenewValue(cachedValue);
          }
          else
          {
            LogError(cacheName, request, ex);
            throw ex;
          }
        }
      }

      return result;
    }

    public static string GetCacheData(string requestXml)
    {
      string sCacheName = GetOuterTagName(requestXml);
      string sValue = string.Empty;
      string sKey = requestXml;
      Cache oCache = _cacheManger.GetGenericDataCache(sCacheName);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sValue = oCacheWrapper.COMAccessClass.GetCacheData(requestXml);
          }

          if (oCache.IsBasedOnPrivateLabelId)
            oCache.AddValue(sKey, sValue, GetPrivateLabelIDFromCallXML(requestXml, oCache.PrivateLabelIdName), oValue);
          else
            oCache.AddValue(sKey, sValue, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            sValue = (string)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        sValue = (string)oValue.Value;

      return sValue;
    }

    public static DataTable GetCacheDataTable(string requestXml)
    {
      DataTable dtResult = null;
      string sCacheName = GetOuterTagName(requestXml);
      string sKey = requestXml;
      Cache oCache = _cacheManger.GetGenericRsCache(sCacheName);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          ADODB.Recordset oRecordSet = null;

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            oRecordSet = (ADODB.Recordset)oCacheWrapper.COMAccessClass.GetCacheRs(requestXml);
          }

          dtResult = GetDataTableFromRecordset(oRecordSet);

          if (oCache.IsBasedOnPrivateLabelId)
            oCache.AddValue(sKey, dtResult, GetPrivateLabelIDFromCallXML(requestXml, oCache.PrivateLabelIdName), oValue);
          else
            oCache.AddValue(sKey, dtResult, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            dtResult = (DataTable)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        dtResult = (DataTable)oValue.Value;

      return dtResult;
    }

    public static void GetCountryData(int countryId,
                                      out string name,
                                      out string abbreviation,
                                      out int callingCode,
                                      out bool isSupported)
    {
      Structs.GetCountryDataValues oCountryValues = new Structs.GetCountryDataValues();
      string sCacheName = "GetCountryData";
      string sKey = countryId.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          object oPvtName, oPvtAbbreviation, oPvtCallingCode, oPvtIsSupported = null;

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            oCacheWrapper.COMAccessClass.GetCountryData(countryId,
                                                    out oPvtName,
                                                    out oPvtAbbreviation,
                                                    out oPvtCallingCode,
                                                    out oPvtIsSupported);
          }


          oCountryValues = new Structs.GetCountryDataValues((string)oPvtName,
                                                            (string)oPvtAbbreviation,
                                                            (int)oPvtCallingCode,
                                                            (bool)oPvtIsSupported);

          oCache.AddValue(sKey, oCountryValues, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            oCountryValues = (Structs.GetCountryDataValues)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
      {
        oCountryValues = (Structs.GetCountryDataValues)oValue.Value;
      }

      abbreviation = oCountryValues.Abbreviation;
      callingCode = oCountryValues.CallingCode;
      isSupported = oCountryValues.IsSupported;
      name = oCountryValues.Name;
    }

    public static DataTable GetCountryList()
    {
      DataTable dtResult = null;
      string sCacheName = "GetCountryList";
      string sKey = sCacheName;
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        if (oValue != null)
        {
          oValue.MarkInProgress();
        }

        string sCountryList = string.Empty;

        try
        {
          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sCountryList = oCacheWrapper.COMAccessClass.GetCountryList();
          }

          dtResult = GetDataTableFromXMLElements("/countries/country", sCountryList);

          oCache.AddValue(sKey, dtResult, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            dtResult = (DataTable)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        dtResult = (DataTable)oValue.Value;

      return dtResult;
    }

    public static string GetCurrencyDataXml(string currencyType)
    {
      string result = null;
      string sCacheName = "GetCurrencyDataXml";
      string sKey = currencyType;
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            result = oCacheWrapper.COMAccessClass.GetCurrencyData(sKey);
          }

          oCache.AddValue(sKey, result, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            result = (string)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        result = (string)oValue.Value;

      return result;
    }

    public static Dictionary<string, Dictionary<string, string>> GetCurrencyDataAll()
    {
      Dictionary<string, Dictionary<string, string>> dictResult = null;
      string sCacheName = "GetCurrencyDataAll";
      string sCurrencyAll = "{all}";
      string sKey = sCurrencyAll;
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          string sXML = string.Empty;
          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sXML = oCacheWrapper.COMAccessClass.GetCurrencyData(sCurrencyAll);
          }

          XmlDocument xdDoc = new XmlDocument();
          xdDoc.LoadXml(sXML);
          XmlNodeList xmlNodeList = xdDoc.SelectNodes("/Cache/currency");

          if (xmlNodeList.Count > 0)
          {
            dictResult = new Dictionary<string, Dictionary<string, string>>(xmlNodeList.Count);

            foreach (XmlNode xmlNode in xmlNodeList)
            {
              XmlElement xmlElement = xmlNode as XmlElement;
              if (xmlElement != null)
              {
                string currencyType = xmlElement.Attributes["gdshop_currencyType"].Value;
                Dictionary<string, string> dictCurrencyTypeData = GetDictionaryFromXMLElement(xmlElement);
                dictResult.Add(currencyType, dictCurrencyTypeData);
              }
            }
          }

          oCache.AddValue(sKey, dictResult, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            dictResult = (Dictionary<string, Dictionary<string, string>>)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        dictResult = (Dictionary<string, Dictionary<string, string>>)oValue.Value;

      return dictResult;
    }

    public static Dictionary<string, string> GetCurrencyData(string sCurrency)
    {
      Dictionary<string, string> dictResult = null;
      string sCacheName = "GetCurrencyData";
      string sKey = sCurrency;
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          string sCurrencyValue = string.Empty;
          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sCurrencyValue = oCacheWrapper.COMAccessClass.GetCurrencyData(sCurrency);
          }

          dictResult = GetDictionaryFromXMLElement("/currency", sCurrencyValue);

          oCache.AddValue(sKey, dictResult, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            dictResult = (Dictionary<string, string>)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        dictResult = (Dictionary<string, string>)oValue.Value;

      return dictResult;
    }

    public static int GetListPrice(int iPrivateLabelID, int iUnifiedPFID, int iPriceTypeID)
    {
      int iPFID = GetPFIDByUnifiedID(iUnifiedPFID, iPrivateLabelID);
      return GetListPrice(iPrivateLabelID, iPFID.ToString(), iPriceTypeID);
    }

    public static int GetListPrice(int iPrivateLabelID, string sPfidOrSku, int iPriceTypeID)
    {
      string sCacheName = "GetListPrice";
      int iListPrice = -1;
      string sKey = iPrivateLabelID.ToString() + "-" + sPfidOrSku + "_" + iPriceTypeID.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            iListPrice = oCacheWrapper.COMAccessClass.GetListPrice(iPrivateLabelID, sPfidOrSku, iPriceTypeID);
          }

          oCache.AddValue(sKey, iListPrice, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            iListPrice = (int)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        iListPrice = (int)oValue.Value;

      return iListPrice;
    }

    public static bool GetListPriceEx(int iPrivateLabelID,
                                      int iUnifiedPFID,
                                      int iPriceTypeID,
                                      string sCurrency,
                                      out int iListPrice,
                                      out bool bEstimate)
    {
      int iPFID = GetPFIDByUnifiedID(iUnifiedPFID, iPrivateLabelID);
      return GetListPriceEx(iPrivateLabelID,
                            iPFID.ToString(),
                            iPriceTypeID,
                            sCurrency,
                            out iListPrice,
                            out bEstimate);
    }

    public static bool GetListPriceEx(int iPrivateLabelID,
                                      string sPfidOrSku,
                                      int iPriceTypeID,
                                      string sCurrency,
                                      out int iListPrice,
                                      out bool bEstimate)
    {
      Structs.GetListPriceExValues oPriceValues = new Structs.GetListPriceExValues();
      string sCacheName = "GetListPriceEx";
      bool bRet = false;
      string sKey = iPrivateLabelID.ToString() + "-" + sPfidOrSku + "-" + iPriceTypeID.ToString() + "-" + sCurrency;
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          object oPvtPrice, oPvtEstimate = null;
          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            bRet = oCacheWrapper.COMAccessClass.GetListPriceEx(iPrivateLabelID,
                                                                sPfidOrSku,
                                                                iPriceTypeID,
                                                                sCurrency,
                                                                out oPvtPrice,
                                                                out oPvtEstimate);
            oPriceValues.IsEstimate = (bool)oPvtEstimate;
            oPriceValues.ListPrice = (int)oPvtPrice;
            oPriceValues.ReturnValue = bRet;
          }

          oCache.AddValue(sKey, oPriceValues, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            oPriceValues = (Structs.GetListPriceExValues)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        oPriceValues = (Structs.GetListPriceExValues)oValue.Value;

      iListPrice = oPriceValues.ListPrice;
      bEstimate = oPriceValues.IsEstimate;

      return oPriceValues.ReturnValue;
    }

    public static void GetMaintNotice(string sWebsite,
                                      out bool bNoticeIsPresent,
                                      out string sNoticeHeader,
                                      out string sNoticeBody)
    {
      Structs.GetMaintNoticeValues oNoticeValues = new Structs.GetMaintNoticeValues();
      string sCacheName = "GetMaintNotice";
      string sKey = sWebsite;
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          object oPvtNoticeIsPresent, oPvtNoticeHeader, oPvtNoticeBody = null;
          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            oCacheWrapper.COMAccessClass.GetMaintNotice(sWebsite,
                                                    out oPvtNoticeIsPresent,
                                                    out oPvtNoticeHeader,
                                                    out oPvtNoticeBody);
            oNoticeValues.NoticeIsPresent = (bool)oPvtNoticeIsPresent;
            oNoticeValues.NoticeHeader = (string)oPvtNoticeHeader;
            oNoticeValues.NoticeBody = (string)oPvtNoticeBody;
          }

          oCache.AddValue(sKey, oNoticeValues, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            oNoticeValues = (Structs.GetMaintNoticeValues)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        oNoticeValues = (Structs.GetMaintNoticeValues)oValue.Value;

      bNoticeIsPresent = oNoticeValues.NoticeIsPresent;
      sNoticeHeader = oNoticeValues.NoticeHeader;
      sNoticeBody = oNoticeValues.NoticeBody;
    }

    public static void GetMgrCategoriesForUser(int iManagerUserID,
                                               out Dictionary<string, string> dictMgrAttributes,
                                               out List<int> lstMgrCategories)
    {
      Structs.GetMgrCategoriesForUserValues oMgrValues = new Structs.GetMgrCategoriesForUserValues();
      string sCacheName = "GetMgrCategoriesForUser";
      string sKey = iManagerUserID.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          string sMgrCategories = string.Empty;
          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sMgrCategories = oCacheWrapper.COMAccessClass.GetMgrCategoriesForUser(iManagerUserID);
          }

          oMgrValues = GetMgrAttrsAndCategoriesFromXML(sMgrCategories);

          oCache.AddValue(sKey, oMgrValues, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            oMgrValues = (Structs.GetMgrCategoriesForUserValues)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        oMgrValues = (Structs.GetMgrCategoriesForUserValues)oValue.Value;

      dictMgrAttributes = oMgrValues.ManagerAttributes;
      lstMgrCategories = oMgrValues.ManagerCategories;
    }

    public static int GetPFIDByUnifiedID(int iUnifiedPFID, int iPrivateLabelID)
    {
      string sCacheName = "GetPFIDByUnifiedID";
      int iPFID = -1;
      string sKey = iUnifiedPFID.ToString() + "-" + iPrivateLabelID.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            iPFID = oCacheWrapper.COMAccessClass.GetPFIDByUnifiedID(iUnifiedPFID, iPrivateLabelID);
          }

          oCache.AddValue(sKey, iPFID, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            iPFID = (int)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        iPFID = (int)oValue.Value;

      return iPFID;
    }

    public static string GetPLData(int iPrivateLabelID, int iDataID)
    {
      string sCacheName = "GetPLData";
      string sPLData = string.Empty;
      string sKey = iPrivateLabelID.ToString() + "-" + iDataID.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sPLData = oCacheWrapper.COMAccessClass.GetPLData(iPrivateLabelID, iDataID);
          }

          oCache.AddValue(sKey, sPLData, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            sPLData = (string)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        sPLData = (string)oValue.Value;

      return sPLData;
    }

    public static string GetPrivateLabelBkColor(int iPrivateLabelID)
    {
      string sCacheName = "GetPrivateLabelBkColor";
      string sPLBKColor = string.Empty;
      string sKey = iPrivateLabelID.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sPLBKColor = oCacheWrapper.COMAccessClass.GetPrivateLabelBkColor(iPrivateLabelID);
          }

          oCache.AddValue(sKey, sPLBKColor, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            sPLBKColor = (string)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        sPLBKColor = (string)oValue.Value;

      return sPLBKColor;
    }

    public static int GetPrivateLabelId(string sProgID)
    {
      string sCacheName = "GetPrivateLabelId";
      int iPrivateLabelID = -1;
      string sKey = sProgID;
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            iPrivateLabelID = oCacheWrapper.COMAccessClass.GetPrivateLabelId(sProgID);
          }

          oCache.AddValue(sKey, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            iPrivateLabelID = (int)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        iPrivateLabelID = (int)oValue.Value;

      return iPrivateLabelID;
    }

    public static string GetPrivateLabelOrderId(int iPrivateLabelID)
    {
      string sCacheName = "GetPrivateLabelOrderId";
      string sValue = string.Empty;
      string sKey = iPrivateLabelID.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sValue = oCacheWrapper.COMAccessClass.GetPrivateLabelOrderId(iPrivateLabelID);
          }

          oCache.AddValue(sKey, sValue, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            sValue = (string)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        sValue = (string)oValue.Value;

      return sValue;
    }

    public static int GetPrivateLabelType(int iPrivateLabelID)
    {
      string sCacheName = "GetPrivateLabelType";
      int iValue = -1;
      string sKey = iPrivateLabelID.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            iValue = oCacheWrapper.COMAccessClass.GetPrivateLabelType(iPrivateLabelID);
          }

          oCache.AddValue(sKey, iValue, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            iValue = (int)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        iValue = (int)oValue.Value;

      return iValue;
    }

    public static IResponseData GetProcessRequest(RequestData oRequestData, int iRequestType)
    {
      IResponseData result = null;
      string sCacheName = "GetProcessRequest" + iRequestType.ToString();
      //TODO: Add Try Catch for MD5
      string sKey = oRequestData.GetCacheMD5();
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue cachedValue = null;
      bool bValid = oCache.TryGetValue(sKey, out cachedValue);

      if (bValid)
      {
        result = (IResponseData)cachedValue.Value;
      }
      else
      {
        if (cachedValue != null)
        {
          cachedValue.MarkInProgress();
        }

        try
        {
          result = Engine.Engine.ProcessRequest(oRequestData, iRequestType);
          oCache.AddValue(sKey, result, 0, cachedValue);
        }
        catch (Exception ex)
        {
          if (cachedValue != null)
          {
            result = (IResponseData)cachedValue.Value;
            oCache.RenewValue(cachedValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }

      return result;
    }

    public static string GetProductDescription(string sPfidOrSku)
    {
      string sCacheName = "GetProductDescription";
      string sValue = string.Empty;
      string sKey = sPfidOrSku;
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sValue = oCacheWrapper.COMAccessClass.GetProductDescription(sPfidOrSku);
          }

          oCache.AddValue(sKey, sValue, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            sValue = (string)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        sValue = (string)oValue.Value;

      return sValue;
    }

    public static string GetProductUpdateXSD(int iXsdID)
    {
      string sCacheName = "GetProductUpdateXSD";
      string sValue = string.Empty;
      string sKey = iXsdID.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sValue = oCacheWrapper.COMAccessClass.GetProductUpdateXSD(iXsdID);
          }

          oCache.AddValue(sKey, sValue, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            sValue = (string)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        sValue = (string)oValue.Value;


      return sValue;
    }

    public static string GetProductXSD(int iXsdID)
    {
      string sCacheName = "GetProductXSD";
      string sValue = string.Empty;
      string sKey = iXsdID.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sValue = oCacheWrapper.COMAccessClass.GetProductXSD(iXsdID);
          }

          oCache.AddValue(sKey, sValue, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            sValue = (string)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        sValue = (string)oValue.Value;

      return sValue;
    }

    public static string GetProgID(int iPrivateLabelID)
    {
      string sCacheName = "GetProgID";
      string sValue = string.Empty;
      string sKey = iPrivateLabelID.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sValue = oCacheWrapper.COMAccessClass.GetProgID(iPrivateLabelID);
          }

          oCache.AddValue(sKey, sValue, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            sValue = (string)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        sValue = (string)oValue.Value;

      return sValue;
    }

    public static int GetPromoPrice(int iPrivateLabelID, int iUnifiedPFID, int iPriceTypeID)
    {
      int iPFID = GetPFIDByUnifiedID(iUnifiedPFID, iPrivateLabelID);
      return GetPromoPrice(iPrivateLabelID, iPFID.ToString(), iPriceTypeID);
    }

    public static int GetPromoPrice(int iPrivateLabelID, string sPfidOrSku, int iPriceTypeID)
    {
      string sCacheName = "GetPromoPrice";
      int iValue = -1;
      string sKey = iPrivateLabelID.ToString() + "-" + sPfidOrSku + "-" + iPriceTypeID.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            iValue = oCacheWrapper.COMAccessClass.GetPromoPrice(iPrivateLabelID, sPfidOrSku, iPriceTypeID);
          }

          oCache.AddValue(sKey, iValue, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            iValue = (int)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
      {
        iValue = (int)oValue.Value;
      }

      return iValue;
    }

    public static int GetPromoPriceByQty(int iPrivateLabelID, int iUnifiedPFID, int iQuantity, int iPriceTypeID)
    {
      int iPFID = GetPFIDByUnifiedID(iUnifiedPFID, iPrivateLabelID);
      return GetPromoPriceByQty(iPrivateLabelID, iPFID.ToString(), iQuantity, iPriceTypeID);
    }

    public static int GetPromoPriceByQty(int iPrivateLabelID, string sPfidOrSku, int iQuantity, int iPriceTypeID)
    {
      string sCacheName = "GetPromoPriceByQty";
      int iValue = -1;
      string sKey = iPrivateLabelID.ToString() + "-" + sPfidOrSku + "-" + iPriceTypeID.ToString() + "-" + iQuantity.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            iValue = oCacheWrapper.COMAccessClass.GetPromoPriceByQty(iPrivateLabelID, sPfidOrSku, iQuantity, iPriceTypeID);
          }

          oCache.AddValue(sKey, iValue, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            iValue = (int)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        iValue = (int)oValue.Value;

      return iValue;
    }

    public static bool GetPromoPriceByQtyEx(int iPrivateLabelID,
                                            int iUnifiedPFID,
                                            int iPriceTypeID,
                                            int iQuantity,
                                            string sCurrency,
                                            out int iPrice,
                                            out bool bEstimate)
    {
      int iPFID = GetPFIDByUnifiedID(iUnifiedPFID, iPrivateLabelID);
      return GetPromoPriceByQtyEx(iPrivateLabelID,
                                  iPFID.ToString(),
                                  iPriceTypeID,
                                  iQuantity,
                                  sCurrency,
                                  out iPrice,
                                  out bEstimate);
    }

    public static bool GetPromoPriceByQtyEx(int iPrivateLabelID,
                                            string sPfidOrSku,
                                            int iPriceTypeID,
                                            int iQuantity,
                                            string sCurrency,
                                            out int iPrice,
                                            out bool bEstimate)
    {
      Structs.GetPromoPriceByQtyExValues oPriceData = new Structs.GetPromoPriceByQtyExValues();
      string sCacheName = "GetPromoPriceByQtyEx";
      bool bRetValue = false;
      string sKey = iPrivateLabelID.ToString() + "-" + sPfidOrSku + "-" + iPriceTypeID.ToString() + "-" + iQuantity.ToString() + "-" + sCurrency;
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            object oPrice, oEstimate = null;
            bRetValue = oCacheWrapper.COMAccessClass.GetPromoPriceByQtyEx(iPrivateLabelID,
                                                                      sPfidOrSku,
                                                                      iQuantity,
                                                                      iPriceTypeID,
                                                                      sCurrency,
                                                                      out oPrice,
                                                                      out oEstimate);

            oPriceData.ReturnValue = bRetValue;
            oPriceData.Price = (int)oPrice;
            oPriceData.IsEstimate = (bool)oEstimate;
          }

          oCache.AddValue(sKey, oPriceData, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            oPriceData = (Structs.GetPromoPriceByQtyExValues)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        oPriceData = (Structs.GetPromoPriceByQtyExValues)oValue.Value;

      bRetValue = oPriceData.ReturnValue;
      bEstimate = oPriceData.IsEstimate;
      iPrice = oPriceData.Price;

      return bRetValue;
    }

    public static bool GetPromoPriceEx(int iPrivateLabelID,
                                       int iUnifiedPFID,
                                       int iPriceTypeID,
                                       string sCurrency,
                                       out int iPrice,
                                       out bool bEstimate)
    {
      int iPFID = GetPFIDByUnifiedID(iUnifiedPFID, iPrivateLabelID);
      return GetPromoPriceEx(iPrivateLabelID,
                             iPFID.ToString(),
                             iPriceTypeID,
                             sCurrency,
                             out iPrice,
                             out bEstimate);
    }

    public static bool GetPromoPriceEx(int iPrivateLabelID,
                                        string sPfidOrSku,
                                        int iPriceTypeID,
                                        string sCurrency,
                                        out int iPrice,
                                        out bool bEstimate)
    {
      string sCacheName = "GetPromoPriceEx";
      bool bRetValue = false;
      Structs.GetPromoPriceByQtyExValues oPriceData = new Structs.GetPromoPriceByQtyExValues();
      string sKey = iPrivateLabelID.ToString() + "-" + sPfidOrSku + "-" + iPriceTypeID.ToString() + "-" + sCurrency;
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            object oPrice, oEstimate = null;
            bRetValue = oCacheWrapper.COMAccessClass.GetPromoPriceEx(iPrivateLabelID,
                                                                  sPfidOrSku,
                                                                  iPriceTypeID,
                                                                  sCurrency,
                                                                  out oPrice,
                                                                  out oEstimate);

            oPriceData.ReturnValue = bRetValue;
            oPriceData.Price = (int)oPrice;
            oPriceData.IsEstimate = (bool)oEstimate;
          }

          oCache.AddValue(sKey, oPriceData, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            oPriceData = (Structs.GetPromoPriceByQtyExValues)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        oPriceData = (Structs.GetPromoPriceByQtyExValues)oValue.Value;

      bRetValue = oPriceData.ReturnValue;
      bEstimate = oPriceData.IsEstimate;
      iPrice = oPriceData.Price;

      return bRetValue;
    }

    public static void GetRelatedIDsForPrivateLabel(int iPrivateLabelID,
                                                    out int iParentPrivateLabelID,
                                                    out int iDefaultTurnkeyID,
                                                    out int iFreeTurnkeyID)
    {
      string sCacheName = "GetRelatedIDsForPrivateLabel";
      Structs.GetRelatedIDsForPrivateLabelValues oRelatedPLIDValues = new Structs.GetRelatedIDsForPrivateLabelValues();
      string sKey = iPrivateLabelID.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            object oParentPLID, oDefaultTK, oFreeTurnKeyID;
            oCacheWrapper.COMAccessClass.GetRelatedIDsForPrivateLabel(iPrivateLabelID,
                                                                  out oParentPLID,
                                                                  out oDefaultTK,
                                                                  out oFreeTurnKeyID);

            oRelatedPLIDValues.ParentPrivateLabelID = (int)oParentPLID;
            oRelatedPLIDValues.DefaultTurnkeyID = (int)oDefaultTK;
            oRelatedPLIDValues.FreeTurnkeyID = (int)oFreeTurnKeyID;
          }

          oCache.AddValue(sKey, oRelatedPLIDValues, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            oRelatedPLIDValues = (Structs.GetRelatedIDsForPrivateLabelValues)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        oRelatedPLIDValues = (Structs.GetRelatedIDsForPrivateLabelValues)oValue.Value;

      iParentPrivateLabelID = oRelatedPLIDValues.ParentPrivateLabelID;
      iDefaultTurnkeyID = oRelatedPLIDValues.DefaultTurnkeyID;
      iFreeTurnkeyID = oRelatedPLIDValues.FreeTurnkeyID;
    }

    public static Dictionary<string, string> GetResellerSampleCommission(string sResellerType, int iPfid)
    {
      Dictionary<string, string> dictResult = null;
      string sCacheName = "GetResellerSampleCommission";
      string sKey = iPfid.ToString() + "-" + sResellerType;
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          string sValue = string.Empty;
          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sValue = oCacheWrapper.COMAccessClass.GetResellerSampleCommission(sResellerType, iPfid);
          }

          dictResult = GetDictionaryFromXMLElement("/PRODUCT ", sValue);

          oCache.AddValue(sKey, dictResult, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            dictResult = (Dictionary<string, string>)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        dictResult = (Dictionary<string, string>)oValue.Value;

      return dictResult;
    }

    public static Dictionary<string, int> GetShopperProduct(string sShopperID)
    {
      Dictionary<string, int> result;
      string sCacheName = "GetShopperProduct";
      string sKey = sShopperID;
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          ADODB.Recordset oRecordSet = null;
          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            oRecordSet = (ADODB.Recordset)oCacheWrapper.COMAccessClass.GetShopperProduct(sShopperID);
          }

          if (oRecordSet != null)
          {
            result = new Dictionary<string, int>(oRecordSet.RecordCount);

            ADODB.Field keyField = oRecordSet.Fields[0];
            ADODB.Field valueField = oRecordSet.Fields[1];

            while (!oRecordSet.EOF)
            {
              int quantity;
              if (Int32.TryParse(valueField.Value.ToString(), out quantity))
              {
                result[keyField.Value.ToString()] = quantity;
              }
              oRecordSet.MoveNext();
            }
          }
          else
          {
            // TODO: using single static empty dictionary for all no recordset results could save memory here
            result = new Dictionary<string, int>(0);
          }

          oCache.AddValue(sKey, result, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            result = (Dictionary<string, int>)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
      {
        result = (Dictionary<string, int>)oValue.Value;
      }

      return result;
    }

    public static void GetShopperRenewingServices(string sShopperID,
                                                  out bool bHasRenewingServices,
                                                  out bool bHasRenewingDomains)
    {
      string sCacheName = "GetShopperRenewingServices";
      Structs.GetShopperRenewingServicesValues oRenewalData = new Structs.GetShopperRenewingServicesValues();
      string sKey = sShopperID;
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          string sValue = string.Empty;
          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sValue = oCacheWrapper.COMAccessClass.GetShopperRenewingServices(sShopperID);
          }

          string[] oValues = sValue.Split('|');
          oRenewalData.HasRenewingServices = Convert.ToInt32(oValues[0]) != 0;
          oRenewalData.HasRenewingDomains = Convert.ToInt32(oValues[1]) != 0;

          oCache.AddValue(sKey, oRenewalData, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            oRenewalData = (Structs.GetShopperRenewingServicesValues)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        oRenewalData = (Structs.GetShopperRenewingServicesValues)oValue.Value;

      bHasRenewingServices = oRenewalData.HasRenewingServices;
      bHasRenewingDomains = oRenewalData.HasRenewingDomains;
    }

    public static void GetStateData(int iStateID, out string sName, out string sAbbreviation, out int iCountryID)
    {
      string sCacheName = "GetStateData";
      Structs.GetStateDataValues oStateData = new Structs.GetStateDataValues();
      string sKey = iStateID.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            object oName, oAbbreviation, oCountryID;
            oCacheWrapper.COMAccessClass.GetStateData(iStateID, out oName, out oAbbreviation, out oCountryID);

            oStateData.Name = (string)oName;
            oStateData.Abbreviation = (string)oAbbreviation;
            oStateData.CountryId = (int)oCountryID;
          }

          oCache.AddValue(sKey, oStateData, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            oStateData = (Structs.GetStateDataValues)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        oStateData = (Structs.GetStateDataValues)oValue.Value;

      sName = oStateData.Name;
      sAbbreviation = oStateData.Abbreviation;
      iCountryID = oStateData.CountryId;
    }

    public static DataTable GetStateList(int iCountryID)
    {
      DataTable dtResult = null;
      string sCacheName = "GetStateList";
      string sKey = iCountryID.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          string sValue = string.Empty;
          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sValue = oCacheWrapper.COMAccessClass.GetStateList(iCountryID);
          }

          dtResult = GetDataTableFromXMLElements("states/state", sValue);

          oCache.AddValue(sKey, dtResult, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            dtResult = (DataTable)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        dtResult = (DataTable)oValue.Value;

      return dtResult;
    }

    public static HashSet<string> GetValidDotTypes()
    {
      HashSet<string> result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
      string sCacheName = "GetValidDotTypes";
      string sKey = "0";

      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          string sValue = string.Empty;
          XmlDocument xdDoc = new XmlDocument();
          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sValue = oCacheWrapper.COMAccessClass.GetTLDData("0");
          }

          xdDoc.LoadXml(sValue);
          XmlNodeList xnlTLDData = xdDoc.SelectNodes("/tldData/data");
          foreach (XmlElement xlTLDData in xnlTLDData)
          {
            string dotType = xlTLDData.GetAttribute("tld");
            if (!string.IsNullOrEmpty(dotType))
            {
              result.Add(dotType);
            }
          }

          oCache.AddValue(sKey, result, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            result = (HashSet<string>)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
      {
        result = (HashSet<string>)oValue.Value;
      }

      return result;
    }

    public static Dictionary<string, Dictionary<string, string>> GetTLDData(string sTldIdOrName)
    {
      Dictionary<string, Dictionary<string, string>> dictResult = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
      string sCacheName = "GetTLDData";
      string sKey = sTldIdOrName;
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          string sValue = string.Empty;
          XmlDocument xdDoc = new XmlDocument();
          string xmlRequest = "<GetTLDInfo><param name=\"tldIdOrName\" value=\"" + sTldIdOrName + "\"/></GetTLDInfo>";

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sValue = oCacheWrapper.COMAccessClass.GetCacheData(xmlRequest);
          }

          xdDoc.LoadXml(sValue);
          XmlNodeList xnlTLDData = xdDoc.SelectNodes("/data/item");
          foreach (XmlElement xlTLDData in xnlTLDData)
          {
            Dictionary<string, string> dictTLDData = GetDictionaryFromXMLElement(xlTLDData);
            dictResult.Add(dictTLDData["tld"], dictTLDData);
          }

          oCache.AddValue(sKey, dictResult, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            dictResult = (Dictionary<string, Dictionary<string, string>>)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
      {
        dictResult = (Dictionary<string, Dictionary<string, string>>)oValue.Value;
      }

      return dictResult;
    }

    public static Dictionary<string, Dictionary<string, string>> GetExtendedTLDData(string sTldIdOrName)
    {
      Dictionary<string, Dictionary<string, string>> dictResult = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
      string sCacheName = "GetTLDData";
      string sKey = sTldIdOrName;
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          string sValue = string.Empty;
          XmlDocument xdDoc = new XmlDocument();
          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sValue = oCacheWrapper.COMAccessClass.GetTLDData(sTldIdOrName);
          }

          xdDoc.LoadXml(sValue);
          foreach (XmlNode node in xdDoc.ChildNodes)
          {
            XmlNodeList xnlTLDData = node.SelectNodes("data");
            foreach (XmlElement xlTLDData in xnlTLDData)
            {
              Dictionary<string, string> dictTLDData = GetDictionaryFromXMLElement(xlTLDData);
              dictResult.Add(dictTLDData["tld"], dictTLDData);
            }
          }

          oCache.AddValue(sKey, dictResult, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            dictResult = (Dictionary<string, Dictionary<string, string>>)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
      {
        dictResult = (Dictionary<string, Dictionary<string, string>>)oValue.Value;
      }

      return dictResult;
    }

    public static DataTable GetTLDList(int iPrivateLabelId, int iProductType)
    {
      DataTable dtResult = null;
      string sCacheName = "GetTLDList";
      string sKey = iPrivateLabelId.ToString() + "-" + iProductType.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          string sValue = string.Empty;
          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sValue = oCacheWrapper.COMAccessClass.GetTLDList(iPrivateLabelId, iProductType);
          }

          dtResult = GetDataTableFromXMLElements("/tldList/tld", sValue);

          oCache.AddValue(sKey, dtResult, iPrivateLabelId, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            dtResult = (DataTable)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        dtResult = (DataTable)oValue.Value;

      return dtResult;
    }

    public static bool IsPrivateLabelActive(int iPrivateLabelID)
    {
      string sCacheName = "IsPrivateLabelActive";
      bool bRetValue = false;
      string sKey = iPrivateLabelID.ToString();
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            bRetValue = oCacheWrapper.COMAccessClass.IsPrivateLabelActive(iPrivateLabelID);
          }

          oCache.AddValue(sKey, bRetValue, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            bRetValue = (bool)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        bRetValue = (bool)oValue.Value;

      return bRetValue;
    }

    public static bool IsProductOnSaleForCurrency(int privateLabelId, int unifiedProductId, string currencyType)
    {
      string cacheName = "IsProductOnSaleForCurrency";
      bool result = false;
      int pfid = GetPFIDByUnifiedID(unifiedProductId, privateLabelId);
      string sKey = string.Concat(privateLabelId.ToString(), "-", pfid.ToString(), "-", currencyType);
      Cache cache = _cacheManger.GetCache(cacheName, true);
      CachedValue cachedValue = null;
      bool isCacheValid = cache.TryGetValue(sKey, out cachedValue);

      if (!isCacheValid)
      {
        try
        {
          if (cachedValue != null)
          {
            cachedValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            result = oCacheWrapper.COMAccessClass.IsProductOnSaleForCurrency(privateLabelId, pfid.ToString(), currencyType);
          }

          cache.AddValue(sKey, result, privateLabelId, cachedValue);
        }
        catch (Exception ex)
        {
          if (cachedValue != null)
          {
            result = (bool)cachedValue.Value;
            cache.RenewValue(cachedValue);
          }
          else
          {
            LogError(cacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        result = (bool)cachedValue.Value;

      return result;
    }

    public static bool IsProductOnSale(int iPrivateLabelID, int iUnifiedPFID)
    {
      int iPFID = GetPFIDByUnifiedID(iUnifiedPFID, iPrivateLabelID);
      return IsProductOnSale(iPrivateLabelID, iPFID.ToString());
    }

    public static bool IsProductOnSale(int iPrivateLabelID, string sPfidOrSku)
    {
      string sCacheName = "IsProductOnSale";
      bool bRetValue = false;
      string sKey = iPrivateLabelID.ToString() + "-" + sPfidOrSku;
      Cache oCache = _cacheManger.GetCache(sCacheName, true);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            bRetValue = oCacheWrapper.COMAccessClass.IsProductOnSale(iPrivateLabelID, sPfidOrSku);
          }

          oCache.AddValue(sKey, bRetValue, iPrivateLabelID, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            bRetValue = (bool)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        bRetValue = (bool)oValue.Value;

      return bRetValue;
    }

    public static string LookupPLData(int iCategory, string sColName)
    {
      string sCacheName = "LookupPLData";
      string sValue = string.Empty;
      string sKey = iCategory.ToString() + "-" + sColName;
      Cache oCache = _cacheManger.GetCache(sCacheName, false);
      CachedValue oValue = null;
      bool bValid = oCache.TryGetValue(sKey, out oValue);

      if (!bValid)
      {
        try
        {
          if (oValue != null)
          {
            oValue.MarkInProgress();
          }

          using (DataCacheWrapper oCacheWrapper = new DataCacheWrapper())
          {
            sValue = oCacheWrapper.COMAccessClass.LookupPLData(iCategory, sColName);
          }

          oCache.AddValue(sKey, sValue, oValue);
        }
        catch (Exception ex)
        {
          if (oValue != null)
          {
            sValue = (string)oValue.Value;
            oCache.RenewValue(oValue);
          }
          else
          {
            LogError(sCacheName, sKey, ex);
            throw ex;
          }
        }
      }
      else
        sValue = (string)oValue.Value;

      return sValue;
    }

    private static DataTable GetDataTableFromRecordset(ADODB.Recordset oRecordset)
    {
      DataTable dtResult = new DataTable();

      if (oRecordset != null && !oRecordset.EOF)
      {
        for (int i = 0; i < oRecordset.Fields.Count; i++)
        {
          string sColumnName = oRecordset.Fields[i].Name;
          dtResult.Columns.Add(sColumnName);
        }

        while (!oRecordset.EOF)
        {
          object[] oDataArray = new object[oRecordset.Fields.Count];
          for (int i = 0; i < oRecordset.Fields.Count; i++)
          {
            oDataArray[i] = oRecordset.Fields[i].Value;
          }

          dtResult.Rows.Add(oDataArray);
          oRecordset.MoveNext();
        }
      }

      return dtResult;
    }

    private static DataTable GetDataTableFromXMLElements(string sXPathToElements, string sXML)
    {
      XmlDocument xdDoc = new XmlDocument();
      DataTable dtResult = new DataTable();

      xdDoc.LoadXml(sXML);
      XmlNodeList xnlElements = xdDoc.SelectNodes(sXPathToElements);

      if (xnlElements.Count > 0)
      {
        foreach (XmlAttribute xaColumn in xnlElements[0].Attributes)
        {
          dtResult.Columns.Add(xaColumn.Name);
        }

        foreach (XmlNode xnElement in xnlElements)
        {
          XmlElement xlElement = xnElement as XmlElement;
          if (xlElement != null)
          {
            object[] oDataRow = new object[xlElement.Attributes.Count];
            for (int i = 0; i < xlElement.Attributes.Count; ++i)
            {
              oDataRow[i] = xlElement.Attributes[i].Value;
            }
            dtResult.Rows.Add(oDataRow);
          }
        }
      }

      return dtResult;
    }

    private static Dictionary<string, string> GetDictionaryFromXMLElement(string sXPathToElement, string sXML)
    {
      XmlDocument xdDoc = new XmlDocument();
      xdDoc.LoadXml(sXML);
      return GetDictionaryFromXMLElement(sXPathToElement, xdDoc);
    }



    private static Dictionary<string, string> GetDictionaryFromXMLElement(string sXPathToElement, XmlDocument xdDoc)
    {
      return GetDictionaryFromXMLElement(xdDoc.SelectSingleNode(sXPathToElement) as XmlElement);
    }

    private static Dictionary<string, string> GetDictionaryFromXMLElement(XmlElement xlElement)
    {
      Dictionary<string, string> dictResult = new Dictionary<string, string>();

      if (xlElement != null)
      {
        foreach (XmlAttribute xaItem in xlElement.Attributes)
          dictResult.Add(xaItem.Name, xaItem.Value);
      }

      return dictResult;
    }

    private static Structs.GetMgrCategoriesForUserValues GetMgrAttrsAndCategoriesFromXML(string sMgrXML)
    {
      Structs.GetMgrCategoriesForUserValues oMgrValues = new Structs.GetMgrCategoriesForUserValues();
      Dictionary<string, string> dictMgrAttributes = new Dictionary<string, string>();
      List<int> lstMgrCategories = new List<int>();
      XmlDocument xdDoc = new XmlDocument();

      oMgrValues.ManagerAttributes = null;
      oMgrValues.ManagerCategories = null;

      xdDoc.LoadXml(sMgrXML);

      dictMgrAttributes = GetDictionaryFromXMLElement("/user", xdDoc);

      XmlNodeList xnlCategories = xdDoc.SelectNodes("/user/category");
      foreach (XmlElement xlCategory in xnlCategories)
        lstMgrCategories.Add(Convert.ToInt32(xlCategory.InnerText));

      oMgrValues.ManagerAttributes = dictMgrAttributes;
      oMgrValues.ManagerCategories = lstMgrCategories;

      return oMgrValues;
    }

    private static string GetOuterTagName(string sXML)
    {
      string sName = string.Empty;
      int iStart = sXML.IndexOf('<');
      int iEnd = sXML.IndexOf('>') - 1;

      if (iStart > -1 && iEnd > -1)
      {
        int iPos = iStart + 1;
        while (iPos < iEnd && !Char.IsWhiteSpace(sXML[iPos]))
          iPos++;
        sName = sXML.Substring(iStart + 1, iPos - iStart);
      }

      return sName;
    }

    private static int GetPrivateLabelIDFromCallXML(string sXML, string sPrivateLabelIDName)
    {
      int iPrivateLabelID = 0;
      XmlDocument xdDoc = new XmlDocument();

      xdDoc.LoadXml(sXML);

      string sXPath = String.Format("//param[@name=\"{0}\"]", sPrivateLabelIDName);
      XmlElement xlParam
        = xdDoc.SelectSingleNode(sXPath) as XmlElement;

      if (xlParam != null)
        iPrivateLabelID = XmlConvert.ToInt32(xlParam.GetAttribute("value"));

      return iPrivateLabelID;
    }

  }
}
