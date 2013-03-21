using Atlantis.Framework.Interface;
using Atlantis.Framework.Rules.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.Rules.Impl
{
  public class RulesMainRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        string rule = ((RulesMainRequestData)requestData).RuleName;
        if (string.IsNullOrEmpty(rule))
        {
          throw new ArgumentException("Rule name cannot be empty or null.");
        }

        string responseXml;
        using (var ruleWebSvc = new ValidationRuleWS.gdValidationRuleWebSvc())
        {
          ruleWebSvc.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          ruleWebSvc.Url = ((WsConfigElement)config).WSURL;
          responseXml = ruleWebSvc.GetRuleMainByName(rule);
        }

        if (!string.IsNullOrEmpty(responseXml))
        {
          XElement responseElement = XElement.Parse(responseXml);
          result = RulesMainResponseData.FromXElement(responseElement);
        }
      }
      catch (Exception ex)
      {
        result = RulesMainResponseData.FromException(requestData, ex);
      }

      return result;
    }
  }
}
