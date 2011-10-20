using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TrafficMobileTracking.Interface
{
  public class TrafficMobileTrackingRequestData : RequestData
  {
    public string MobileDeviceUid { get; private set; }

    public short MobileApplicationId { get; private set; }

    public string ApplicationVersion { get; private set; }

    public string OperatingSystem { get; private set; }

    public string OperatingSystemVersion { get; private set; }

    public string DeviceModel { get; private set; }

    public string DeviceType { get; private set; }

    public string MethodName { get; private set; }

    public string ClientIp { get; private set; }

    public string ClientCarrier { get; private set; }

    public string LogData { get; private set; }

    public int? CiCode { get; set; }
    
    public int PrivateLabelId { get; private set; }
    
    public TrafficMobileTrackingRequestData(string sShopperID, 
                                            int privateLabelId,
                                            string mobileDeviceUid,
                                            short mobileApplicationId,
                                            string applicationVersion,
                                            string operatingSystem,
                                            string operatingSystemVersion,
                                            string deviceModel,
                                            string deviceType,
                                            string methodName,
                                            string clientIp,
                                            string clientCarrier,
                                            string logData,
                                            string sSourceURL, 
                                            string sOrderID, 
                                            string sPathway, 
                                            int iPageCount) : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      PrivateLabelId = privateLabelId;
      MobileDeviceUid = mobileDeviceUid;
      MobileApplicationId = mobileApplicationId;
      ApplicationVersion = applicationVersion;
      OperatingSystem = operatingSystem;
      OperatingSystemVersion = operatingSystemVersion;
      DeviceModel = deviceModel;
      DeviceType = deviceType;
      MethodName = methodName;
      ClientIp = clientIp;
      ClientCarrier = clientCarrier;
      LogData = logData;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public TrafficMobileTrackingRequestData(string sShopperID,
                                            int privateLabelId,
                                            string mobileDeviceUid,
                                            short mobileApplicationId,
                                            string applicationVersion,
                                            string operatingSystem,
                                            string operatingSystemVersion,
                                            string deviceModel,
                                            string deviceType,
                                            string methodName,
                                            string clientIp,
                                            string clientCarrier,
                                            string logData,
                                            int ciCode,
                                            string sSourceURL,
                                            string sOrderID,
                                            string sPathway,
                                            int iPageCount) : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      PrivateLabelId = privateLabelId;
      MobileDeviceUid = mobileDeviceUid;
      MobileApplicationId = mobileApplicationId;
      ApplicationVersion = applicationVersion;
      OperatingSystem = operatingSystem;
      OperatingSystemVersion = operatingSystemVersion;
      DeviceModel = deviceModel;
      DeviceType = deviceType;
      MethodName = methodName;
      ClientIp = clientIp;
      ClientCarrier = clientCarrier;
      LogData = logData;
      CiCode = ciCode;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      throw new Exception("TrafficMobileTracking is not a cacheable request.");
    }
  }
}
