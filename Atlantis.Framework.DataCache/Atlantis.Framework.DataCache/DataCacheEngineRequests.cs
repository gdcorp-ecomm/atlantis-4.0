using Atlantis.Framework.AppSettings.Interface;
using Atlantis.Framework.DataCacheGeneric.Interface;
using Atlantis.Framework.PrivateLabel.Interface;
using Atlantis.Framework.Products.Interface;

namespace Atlantis.Framework.DataCache
{
  public static class DataCacheEngineRequests
  {
    public static int GetAppSetting { get; set; }
    public static int GetPrivateLabelData { get; set; }
    public static int GetPrivateLabelId { get; set; }
    public static int GetPrivateLabelType { get; set; }
    public static int GetIsPrivateLabelActive { get; set; }
    public static int GetProgId { get; set; }
    public static int GetCacheData { get; set; }
    public static int GetNonUnifiedPfid { get; set; }

    static DataCacheEngineRequests()
    {
      GetAppSetting = 658;
      GetPrivateLabelData = 659;
      GetPrivateLabelId = 660;
      GetProgId = 661;
      GetPrivateLabelType = 662;
      GetIsPrivateLabelActive = 663;
      GetCacheData = 694;
      GetNonUnifiedPfid = 699;
    }

    internal static string ExecuteAppSetting(string settingName)
    {
      var request = new AppSettingRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, settingName);
      var response = (AppSettingResponseData)DataCache.GetProcessRequest(request, GetAppSetting);
      return response.SettingValue;
    }

    internal static string ExecuteGetPrivateLabelData(int privateLabelId, int dataCategoryId)
    {
      var request = new PrivateLabelDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, privateLabelId, dataCategoryId);
      var response = (PrivateLabelDataResponseData)DataCache.GetProcessRequest(request, GetPrivateLabelData);
      return response.DataValue;
    }

    internal static int ExecuteGetPrivateLabelId(string progId)
    {
      var request = new PrivateLabelIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, progId);
      var response = (PrivateLabelIdResponseData)DataCache.GetProcessRequest(request, GetPrivateLabelId);
      return response.PrivateLabelId;
    }

    internal static int ExecuteGetPrivateLabelType(int privateLabelId)
    {
      var request = new PrivateLabelTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, privateLabelId);
      var response = (PrivateLabelTypeResponseData)DataCache.GetProcessRequest(request, GetPrivateLabelType);
      return response.PrivateLabelType;
    }

    internal static bool ExecuteIsPrivateLabelActive(int privateLabelId)
    {
      var request = new IsPrivateLabelActiveRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, privateLabelId);
      var response = (IsPrivateLabelActiveResponseData)DataCache.GetProcessRequest(request, GetIsPrivateLabelActive);
      return response.IsActive;
    }

    internal static string ExecuteGetProgId(int privateLabelId)
    {
      var request = new ProgIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, privateLabelId);
      var response = (ProgIdResponseData)DataCache.GetProcessRequest(request, GetProgId);
      return response.ProgId;
    }

    internal static string ExecuteGetCacheData(string requestXml)
    {
      var request = new GetCacheDataRequestData(requestXml);
      var response = (GetCacheDataResponseData)DataCache.GetProcessRequest(request, GetCacheData);
      return response.ToXML();
    }

    internal static int ExecuteGetNonunifiedPfid(int unifiedProductId, int privateLabelId)
    {
      var request = new NonUnifiedPfidRequestData(unifiedProductId, privateLabelId);
      var response = (NonUnifiedPfidResponseData)DataCache.GetProcessRequest(request, GetNonUnifiedPfid);
      return response.NonUnifiedPfid;
    }
  }
}
