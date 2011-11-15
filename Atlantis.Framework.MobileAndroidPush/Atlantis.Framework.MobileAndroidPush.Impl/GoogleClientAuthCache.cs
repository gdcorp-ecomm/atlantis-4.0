using Atlantis.Framework.GoogleClientAuth.Interface;
using Atlantis.Framework.MobileAndroidPush.Interface;

namespace Atlantis.Framework.MobileAndroidPush.Impl
{
  public class GoogleClientAuthCache
  {
    private const string APPLICATION_NAME = "GoDaddy-MobilePushService";
    private const int GOOGLE_CLIENT_AUTH_REQUEST_ID = 444;

    private static volatile string _authToken;
    private static volatile bool _clearCache;

    public bool ClearCache
    {
      get { return _clearCache; }
      set { _clearCache = value; }
    }

    public string GetClientAuthToken(MobileAndroidPushRequestData mobileAndroidPushRequestData)
    {
      if (ClearCache || string.IsNullOrEmpty(_authToken))
      {
        using(new MonitorLock(this))
        {
          if (ClearCache || string.IsNullOrEmpty(_authToken))
          {
            ClearCache = false;
            GoogleClientAuthRequestData requestData = new GoogleClientAuthRequestData(GoogleClientAuthServiceType.AndroidPush,
                                                                                      GoogleClientAuthAccountType.Google,
                                                                                      APPLICATION_NAME,
                                                                                      mobileAndroidPushRequestData.ShopperID,
                                                                                      mobileAndroidPushRequestData.SourceURL,
                                                                                      mobileAndroidPushRequestData.OrderID,
                                                                                      mobileAndroidPushRequestData.Pathway,
                                                                                      mobileAndroidPushRequestData.PageCount);

            GoogleClientAuthResponseData responseData = (GoogleClientAuthResponseData)Engine.Engine.ProcessRequest(requestData, GOOGLE_CLIENT_AUTH_REQUEST_ID);
            if(responseData.IsSuccess)
            {
              _authToken = responseData.ClientAuthToken;
            }
          }
        }
      }
      return _authToken;
    }
  }
}
