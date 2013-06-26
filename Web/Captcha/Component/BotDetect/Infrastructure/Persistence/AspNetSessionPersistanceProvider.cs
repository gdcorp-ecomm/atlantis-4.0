using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;
using System.Diagnostics;
using System.Text;

namespace BotDetect.Persistence
{
  /// <summary>
  /// Summary description for SessionPersistanceProvider.
  /// </summary>
  internal class AspNetSessionPersistenceProvider : PersistenceProvider, IPersistenceProvider, ICollection
  {
    private readonly string _keyNameSpace = "SessionPersistanceProvider_";

    // singleton
    private static readonly AspNetSessionPersistenceProvider _instance =
        new AspNetSessionPersistenceProvider();

    public static AspNetSessionPersistenceProvider Persistence
    {
      get
      {
        return _instance;
      }
    }

    private AspNetSessionPersistenceProvider()
    {
    }

    private string GetSessionKey(string key)
    {
      return _keyNameSpace + key;
    }

    public override void Save(string key, object val)
    {
      try
      {
        string modifiedKey = GetSessionKey(key);

        if (null != HttpContext.Current.Session)
        {
          HttpContext.Current.Session[modifiedKey] = val;
        }
      }
      catch (Exception ex)
      {
        Debug.Assert(false, ex.Message);
        throw;
      }
    }

    public override object Load(string key)
    {
      try
      {
        string modifiedKey = GetSessionKey(key);

        if (null != HttpContext.Current.Session)
        {
          return HttpContext.Current.Session[modifiedKey];
        }

        return null;
      }
      catch (Exception ex)
      {
        Debug.Assert(false, ex.Message);
        throw;
      }
    }

    public override bool Contains(string key)
    {
      try
      {
        string modifiedKey = GetSessionKey(key);

        if (null != HttpContext.Current.Session &&
            null != HttpContext.Current.Session[modifiedKey])
        {
          return true;
        }

        return false;
      }
      catch (Exception ex)
      {
        Debug.Assert(false, ex.Message);
        throw;
      }
    }

    public override void Remove(string key)
    {
      try
      {
        string modifiedKey = GetSessionKey(key);

        if (null != HttpContext.Current.Session &&
            null != HttpContext.Current.Session[modifiedKey])
        {
          HttpContext.Current.Session.Remove(modifiedKey);
        }

      }
      catch (Exception ex)
      {
        Debug.Assert(false, ex.Message);
        throw;
      }
    }

    public override void Clear()
    {
      try
      {
        if (null != HttpContext.Current.Session)
        {
          HttpContext.Current.Session.Clear();
        }
      }
      catch (Exception ex)
      {
        Debug.Assert(false, ex.Message);
        throw;
      }
    }

    public override void CopyTo(Array array, int index)
    {
      if (null != HttpContext.Current.Session)
      {
        HttpContext.Current.Session.CopyTo(array, index);
      }
    }

    public override int Count
    {
      get
      {
        if (null != HttpContext.Current.Session)
        {
          return HttpContext.Current.Session.Count;
        }

        return 0;
      }
    }

    public override bool IsSynchronized
    {
      get
      {
        if (null != HttpContext.Current.Session)
        {
          return HttpContext.Current.Session.IsSynchronized;
        }

        return false;
      }
    }

    public override object SyncRoot
    {
      get
      {
        if (null != HttpContext.Current.Session)
        {
          return HttpContext.Current.Session.SyncRoot;
        }

        return null;
      }
    }

    public override NameObjectCollectionBase.KeysCollection Keys
    {
      get
      {
        if (null != HttpContext.Current.Session)
        {
          return HttpContext.Current.Session.Keys;
        }

        return null;
      }
    }

    public override IEnumerator GetEnumerator()
    {
      if (null != HttpContext.Current.Session)
      {
        return HttpContext.Current.Session.GetEnumerator();
      }

      return null;
    }

    public override string ToString()
    {
      StringBuilder str = new StringBuilder();
      str.Append("BotDetect.Persistence.AspNetSessionPersistenceProvider {");

      if (null == HttpContext.Current.Session)
      {
        str.Append(" null }");
      }
      else
      {
        str.AppendLine();
        HttpSessionState session = HttpContext.Current.Session;

        str.AppendLine("  session id: " + session.SessionID);
        str.AppendLine("  new session: " + session.IsNewSession);

        str.AppendLine("  mode: " + session.Mode);
        str.AppendLine("  timeout: " + session.Timeout);

        str.AppendLine("  cookie mode: " + session.CookieMode);
        str.AppendLine("  cookieless: " + session.IsCookieless);

        str.AppendLine("  codepage: " + session.CodePage);
        str.AppendLine("  locale id: " + session.LCID);

        str.AppendLine("  saved elements: " + session.Count);

        str.Append("}");
      }

      return str.ToString();
    }
  }
}
