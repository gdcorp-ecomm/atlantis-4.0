using System;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TrafficPageEvent.Interface
{
  public class TrafficPageEventRequestData : RequestData
  {
    public string PageName { get; private set; }

    public int CiCode { get; private set; }

    public string EventType { get; private set; }

    private StringBuilder _ciImpressions;
    public string CiImpressions
    {
      get
      {
        string value;
        if (_ciImpressions == null)
        {
          value = string.Empty;
        }
        else
        {
          value = _ciImpressions.ToString();
        }
        return value;
      }
    }
    

    private StringBuilder _userKeyValuePairsString;
    public string UserKeyValuePairsString
    {
      get
      {
        string value;
        if(_userKeyValuePairsString == null)
        {
          value = string.Empty;
        }
        else
        {
          value = _userKeyValuePairsString.ToString();
        }
        return value;
      }
    }

    public TrafficPageEventRequestData(string pageName,
                                       int ciCode,
                                       string eventType,
                                       string sShopperID, 
                                       string sSourceURL, 
                                       string sOrderID, 
                                       string sPathway, 
                                       int iPageCount) : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      PageName = pageName ?? string.Empty;
      CiCode = ciCode;
      EventType = eventType ?? string.Empty;
      RequestTimeout = new TimeSpan(0, 0, 5);
    }

    public void AddCiImpression(int ciCode)
    {
      if (ciCode > 0)
      {
        if (_ciImpressions == null)
        {
          _ciImpressions = new StringBuilder(ciCode.ToString());
        }
        else
        {
          _ciImpressions.AppendFormat(",{0}", ciCode);
        }
      }
    }

    public void AddKeyValuePair(string key, string value)
    {
      if(!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
      {
        if(_userKeyValuePairsString == null)
        {
          _userKeyValuePairsString = new StringBuilder(string.Format("{0},{1}", key, value));
        }
        else
        {
          _userKeyValuePairsString.AppendFormat(",{0},{1}", key, value);
        }
      }
    }

    public override string GetCacheMD5()
    {
      throw new Exception("TrafficPageEvent is not a cacheable request.");
    }
  }
}
