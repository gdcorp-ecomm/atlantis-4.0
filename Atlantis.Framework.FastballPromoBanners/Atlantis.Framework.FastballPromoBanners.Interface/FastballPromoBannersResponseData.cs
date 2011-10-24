using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.FastballPromoBanners.Interface
{
  public class FastballPromoBannersResponseData : IResponseData, ISessionSerializableResponse
  {
    public bool IsSuccess { get; private set; }

    public AtlantisException AtlantisException { get; private set; }

    public IList<FastballPromoBanner> FastballPromoBanners { get; private set; }

    public FastballPromoBannersResponseData()
    {
    }

    public FastballPromoBannersResponseData(RequestData requestData, Exception ex)
    {
      AtlantisException = new AtlantisException(requestData, "Atlantis.Framework.FastballPromoBanners", ex.Message + "|" + ex.StackTrace, string.Empty, ex);
      FastballPromoBanners = new List<FastballPromoBanner>(1);
      IsSuccess = false;
    }

    public FastballPromoBannersResponseData(IList<FastballPromoBanner> fastballPromoBanners)
    {
      if (fastballPromoBanners != null && fastballPromoBanners.Count > 0)
      {
        FastballPromoBanners = fastballPromoBanners;
        IsSuccess = true;
      }
      else
      {
        FastballPromoBanners = new List<FastballPromoBanner>(1);
        IsSuccess = false;
      }
    }

    public string ToXML()
    {
      return SerializeSessionData();
    }

    public AtlantisException GetException()
    {
      return AtlantisException;
    }

    public string SerializeSessionData()
    {
      string sessionString;
      MemoryStream ms = new MemoryStream();
      DataContractJsonSerializer ser;

      try
      {
        ser = new DataContractJsonSerializer(typeof(List<FastballPromoBanner>));
        ser.WriteObject(ms, FastballPromoBanners);
        sessionString = Encoding.Default.GetString(ms.ToArray());
        ms.Close();
      }
      catch
      {
        sessionString = null;
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
        ser = new DataContractJsonSerializer(typeof(List<FastballPromoBanner>));
        FastballPromoBanners = ser.ReadObject(ms) as List<FastballPromoBanner>;
        IsSuccess = FastballPromoBanners != null && FastballPromoBanners.Count > 0;
        ms.Close();
      }
      finally
      {
        if (ms != null)
        {
          ms.Dispose();
        }
      }
    }
  }
}
