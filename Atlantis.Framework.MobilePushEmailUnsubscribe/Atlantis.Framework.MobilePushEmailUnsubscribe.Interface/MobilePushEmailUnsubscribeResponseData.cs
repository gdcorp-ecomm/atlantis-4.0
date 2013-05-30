using System;
using System.Reflection;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.MobilePushEmailUnsubscribe.Interface
{
  public class MobilePushEmailUnsubscribeResponseData :  EccResponseDataBase<object>, ISessionSerializableResponse
  {
    private AtlantisException AtlantisException { get; set; }

    public MobilePushEmailUnsubscribeResponseData()
    {
    }

    public MobilePushEmailUnsubscribeResponseData(string resultJson)
      : base(resultJson)
    {
    }

    public MobilePushEmailUnsubscribeResponseData(RequestData requestData, Exception ex)
    {
      IsSuccess = false;
      AtlantisException = new AtlantisException(requestData,
                                                MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                ex.Message,
                                                ex.StackTrace);
    }

    #region Implementation of ISessionSerializableResponse

    public string SerializeSessionData()
    {
      throw new NotImplementedException();
    }

    public void DeserializeSessionData(string sessionData)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
