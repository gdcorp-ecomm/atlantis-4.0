using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.UserAgentEx.Interface
{
  public class UserAgentExResponseData : IResponseData
  {
    private readonly List<Regex> _expressions = null;
    private AtlantisException _exception = null;

    public UserAgentExResponseData(IEnumerable<Regex> expressions)
    {
      _expressions = new List<Regex>(expressions);
    }

    public UserAgentExResponseData(RequestData request, Exception ex)
    {
      _exception = new AtlantisException(request, "UserAgentExResponseData.ctor", ex.Message, ex.StackTrace, ex);
    }

    public bool IsMatch(string userAgent)
    {
      bool result = false;
      if (!string.IsNullOrEmpty(userAgent))
      {
        foreach (Regex expression in _expressions)
        {
          if (expression.IsMatch(userAgent))
          {
            result = true;
            break;
          }
        }
      }
      return result;
    }

    public Match FindMatch(string userAgent)
    {
      Match result = Match.Empty;
      if (!string.IsNullOrEmpty(userAgent))
      {
        foreach (Regex expression in _expressions)
        {
          if (expression.IsMatch(userAgent))
          {
            result = expression.Match(userAgent);
            break;
          }
        }
      }
      return result;
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
