using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.FastballGetOffersMsgData.Interface
{
  public class FastballGetOffersMsgDataResponseData : IResponseData, ISessionSerializableResponse
  {
    private AtlantisException _exception;
    
    public bool IsSuccess{ get; set; }
    
    public IList<FastBallBannerAd> FastBallAds { get; set; }

    public FastballGetOffersMsgDataResponseData()
    {
    }

    public FastballGetOffersMsgDataResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(
        requestData, "Atlantis.Framework.FastballGetOffersMsgData", ex.Message, ex.StackTrace, ex);
      IsSuccess = false;
    }

    public FastballGetOffersMsgDataResponseData(List<FastBallBannerAd> inAds)
    {
      FastBallAds = inAds;
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string SerializeSessionData()
    {
      string sessionString = string.Empty;
      MemoryStream ms = new MemoryStream();
      DataContractJsonSerializer ser;

      try
      {
        ser = new DataContractJsonSerializer(typeof(IList<FastBallBannerAd>));
        ser.WriteObject(ms, FastBallAds);
        sessionString = Encoding.Default.GetString(ms.ToArray());
        ms.Close();
      }
      finally
      {
        ms.Dispose();
      }

      return sessionString;
    }

    public void DeserializeSessionData(string sessionData)
    {
      MemoryStream ms = null;
      DataContractJsonSerializer ser;

      try
      {
        ms = new MemoryStream(Encoding.Unicode.GetBytes(sessionData));
        ser = new DataContractJsonSerializer(typeof(IList<FastBallBannerAd>));
        FastBallAds = ser.ReadObject(ms) as IList<FastBallBannerAd>;
        IsSuccess = true;
        ms.Close();
      }
      finally
      {
        if(ms != null)
        {
          ms.Dispose();
        }
      }
    }
  }
}
