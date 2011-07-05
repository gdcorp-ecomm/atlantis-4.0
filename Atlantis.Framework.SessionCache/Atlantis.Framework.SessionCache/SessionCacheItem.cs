using System;
using System.Web.UI;

namespace Atlantis.Framework.SessionCache
{
  public class SessionCacheItem
  {
    public bool IsValid { get; private set; }
    public string SessionData { get; private set; }
    public DateTime Expires { get; private set; }

    public bool IsExpired
    {
      get { return DateTime.Now > Expires; }
    }

    internal SessionCacheItem(Pair sessionPair)
    {
      IsValid = false;

      if (sessionPair != null)
      {
        if ((sessionPair.First != null) && (sessionPair.First.GetType() == typeof(string)))
        {
          SessionData = sessionPair.First.ToString();
          IsValid = true;

          if ((sessionPair.Second != null) && (sessionPair.Second.GetType() == typeof(DateTime)))
          {
            Expires = Convert.ToDateTime(sessionPair.Second);
          }
          else
          {
            Expires = DateTime.MaxValue;
          }
        }
      }
    }

    internal Pair RefreshedSessionPair(TimeSpan cacheTime)
    {
      Pair result;
      if (cacheTime == TimeSpan.MaxValue)
      {
        result = NewSessionPair(SessionData);
      }
      else
      {
        result = NewSessionPair(SessionData, cacheTime);
      }

      return result;
    }

    internal static Pair NewSessionPair(string sessionData, TimeSpan cacheTime)
    {
      DateTime expires = DateTime.Now.Add(cacheTime);
      Pair result = new Pair(sessionData, expires);
      return result;
    }

    internal static Pair NewSessionPair(string sessionData)
    {
      Pair result = new Pair(sessionData, null);
      return result;
    }

  }
}
