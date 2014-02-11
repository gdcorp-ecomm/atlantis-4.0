using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Atlantis.Framework.DataCache
{
  public static class DataCache
  {
    const int MCAST_PORT = 7813;
    const string MCAST_ADDR = "224.7.8.13";

    static CacheManager<object> _customObjectCacheManager;
    static CacheManager<IResponseData> _responseDataCacheManager;

    static DataCache()
    {
      try
      {
        _customObjectCacheManager = new CacheManager<object>();
        _responseDataCacheManager = new CacheManager<IResponseData>();

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
      try
      {
        int index = request.IndexOf('/');
        if (index > -1)
        {
          request = request.Substring(0, index);
        }

        if (!string.IsNullOrEmpty(request))
        {
          var clearParameters = request.Split('|');
          if (clearParameters.Length == 1 || string.IsNullOrEmpty(clearParameters[1]))
          {
            _customObjectCacheManager.ClearCacheData(request);
            _responseDataCacheManager.ClearCacheData(request);
          }
          else
          {
            _responseDataCacheManager.ClearCacheData(clearParameters[0], clearParameters[1]);
          }
        }
      }
      catch (ThreadAbortException)
      { }
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

    [Obsolete("Please take the time to create a cacheable triplet and using ClearCachedData instead of using this method. This method will be removed in a future version.")]
    public static void ClearCustomCachedData<T>()
    {
      _customObjectCacheManager.ClearCacheData(CustomCacheName<T>());
    }

    public static void ClearCachedData(int requestType)
    {
      _responseDataCacheManager.ClearCacheData(ResponseCacheName(requestType));
    }

    private static string ResponseCacheName(int requestType)
    {
      return "GetProcessRequest" + requestType.ToString();
    }

    private static string CustomCacheName<T>()
    {
      return "CustomCacheData." + typeof(T).FullName;
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

    public delegate T GetCustomDataDelegate<T>(string requestKey);

    [Obsolete("Please take the time to create a cacheable triplet instead of using this method. This method will be removed in a future version.")]
    public static T GetCustomCacheData<T>(string request, GetCustomDataDelegate<T> getCustomDataForCache)
    {
      T result = default(T);
      string cacheName = CustomCacheName<T>();
      Cache<object> cache = _customObjectCacheManager.GetCache(cacheName);
      CachedValue<object> cachedValue = null;
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

          cache.AddValue(request, result, cachedValue);
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

    public static IResponseData GetProcessRequest(RequestData requestData, int requestType)
    {
      IResponseData result = null;
      string cacheName = ResponseCacheName(requestType);
      string cacheKey = requestData.GetCacheMD5();
      Cache<IResponseData> cache = _responseDataCacheManager.GetCache(cacheName);
      CachedValue<IResponseData> cachedValue = null;
      bool isValid = cache.TryGetValue(cacheKey, out cachedValue);

      if (isValid)
      {
        result = cachedValue.Value;
      }
      else
      {
        if (cachedValue != null)
        {
          cachedValue.MarkInProgress();
        }

        try
        {
          result = Engine.Engine.ProcessRequest(requestData, requestType);
          cache.AddValue(cacheKey, result, cachedValue);
        }
        catch (Exception ex)
        {
          if (cachedValue != null)
          {
            result = cachedValue.Value;
            cache.RenewValue(cachedValue);
          }
          else
          {
            LogError(cacheName, cacheKey, ex);
            throw ex;
          }
        }
      }

      return result;
    }

    [Obsolete("This method will be removed. Please use the DataCacheGeneric Triplet instead or consider creating a proper triplet for your generic cache data.")]
    public static string GetCacheData(string requestXml)
    {
      return DataCacheEngineRequests.ExecuteGetCacheData(requestXml);
    }

    [Obsolete("This method will be removed. Please use the AppSettings Triplet directly.")]
    public static string GetAppSetting(string settingName)
    {
      return DataCacheEngineRequests.ExecuteAppSetting(settingName);
    }

    [Obsolete("This method will be removed. Please use the PrivateLabel provider's IPrivateLabel.GetPrivateLabelData method or PrivateLabelDataRequest of the PrivateLabel triplet directly.")]
    public static string GetPLData(int privateLabelId, int dataCategoryId)
    {
      return DataCacheEngineRequests.ExecuteGetPrivateLabelData(privateLabelId, dataCategoryId);
    }

    [Obsolete("This method will be removed. Please use the PrivateLabel provider's IPrivateLabel.PrivateLabelId property or PrivateLabelIdRequest of the PrivateLabel triplet directly.")]
    public static int GetPrivateLabelId(string progId)
    {
      return DataCacheEngineRequests.ExecuteGetPrivateLabelId(progId);
    }

    [Obsolete("This method will be removed. Please use the PrivateLabel provider's IPrivateLabel.PrivateLabelType property or PrivateLabelTypeRequest of the PrivateLabel triplet directly.")]
    public static int GetPrivateLabelType(int privateLabelId)
    {
      return DataCacheEngineRequests.ExecuteGetPrivateLabelType(privateLabelId);
    }

    [Obsolete("This method will be removed. Please use the PrivateLabel provider's IPrivateLabel.ProgId property or ProgIdRequest of the PrivateLabel triplet directly.")]
    public static string GetProgID(int privateLabelId)
    {
      return DataCacheEngineRequests.ExecuteGetProgId(privateLabelId);
    }

    [Obsolete("This method will be removed. Please use the PrivateLabel provider's IPrivateLabel.IsActive property or IsPrivateLabelActiveRequest of the PrivateLabel triplet directly.")]
    public static bool IsPrivateLabelActive(int privateLabelId)
    {
      return DataCacheEngineRequests.ExecuteIsPrivateLabelActive(privateLabelId);
    }

    [Obsolete("This method will be removed. Please use the IProductProvider.GetNonUnifiedPfid or the NonUnifiedPfidRequestData in the Products triplet directly.")]
    public static int GetPFIDByUnifiedID(int unifiedProductId, int privateLabelId)
    {
      return DataCacheEngineRequests.ExecuteGetNonunifiedPfid(unifiedProductId, privateLabelId);
    }
  }
}
