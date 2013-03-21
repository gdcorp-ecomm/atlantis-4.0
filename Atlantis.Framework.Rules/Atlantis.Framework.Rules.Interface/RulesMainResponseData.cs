using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;

namespace Atlantis.Framework.Rules.Interface
{
  public class RulesMainResponseData : IResponseData
  {
    public static RulesMainResponseData Empty { get; private set; }

    static RulesMainResponseData()
    {
      Empty = new RulesMainResponseData();
    }

    private RulesMainResponseData()
    {
      _rulesXDoc = new XDocument();
    }

    private readonly AtlantisException _exception;
    private readonly XElement _responseXml;

    private readonly XDocument _rulesXDoc;
    public XDocument Rules
    {
      get { return _rulesXDoc; }
    }

    public static RulesMainResponseData FromException(RequestData requestData, Exception ex)
    {
      return new RulesMainResponseData(requestData, ex);
    }

    private RulesMainResponseData(RequestData requestData, Exception ex)
    {
      string message = ex.Message + ex.StackTrace;
      string inputData = requestData.ToXML();
      _exception = new AtlantisException(requestData, "RulesMainResponseData.ctor", message, inputData);
    }

    public static RulesMainResponseData FromXElement(XElement responseXml)
    {
      return new RulesMainResponseData(responseXml);
    }

    private RulesMainResponseData(XElement responseXml)
    {
      _responseXml = responseXml;

      var successResult = _responseXml.Attribute("success");
      if (!"true".Equals(successResult.Value, StringComparison.OrdinalIgnoreCase))
      {
        throw new ArgumentException("response success result was not 'true'.");
      }

      var ruleEnginesElement = _responseXml.Element("RuleEngines");
      if (ruleEnginesElement != null)
      {
        _rulesXDoc = XDocument.Parse(ruleEnginesElement.ToString());
      }
    }

    public string ToXML()
    {
      return _rulesXDoc.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
