using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.UserAgentEx.Interface;

namespace Atlantis.Framework.UserAgentEx.Impl
{
  public class UserAgentExRequest : IRequest
  {
    const string _USERAGENTEXREQUEST = "<GetUserAgentExpressionByTypeID><param name=\"n_userAgent_expressionTypeID\" value=\"{0}\"/></GetUserAgentExpressionByTypeID>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      UserAgentExResponseData result;

      try
      {
        UserAgentExRequestData request = (UserAgentExRequestData)requestData;
        string cacheRequest = string.Format(_USERAGENTEXREQUEST, request.ExpressionType.ToString());
        string resultXml = DataCache.DataCache.GetCacheData(cacheRequest);

        List<Regex> expressions = new List<Regex>(25);
        XDocument userAgentDoc = XDocument.Parse(resultXml);

        foreach (XElement element in userAgentDoc.Descendants("item"))
        {
          XAttribute regexAtt = element.Attribute("userAgent_expression");
          if (regexAtt != null)
          {
            Regex regex = new Regex(regexAtt.Value, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            expressions.Add(regex);
          }
        }

        result = new UserAgentExResponseData(expressions);

      }
      catch (Exception ex)
      {
        result = new UserAgentExResponseData(requestData, ex);
      }

      return result;

    }
  }
}
