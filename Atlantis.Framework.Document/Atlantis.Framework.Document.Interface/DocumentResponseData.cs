using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Document.Interface
{
  public class DocumentResponseData : IResponseData
  {
    private readonly Regex _styleEx;
    private readonly Regex _bodyEx;
    private readonly Regex _classEx;
    private readonly AtlantisException _exception;

    public DocumentResponseData(string html)
    {
      Html = !string.IsNullOrEmpty(html) ? html : String.Empty;
      Body = String.Empty;
      Styles = String.Empty;
      BodyStyle = String.Empty;
      _styleEx = new Regex("<style.*?>(?<styles>.*?)</style>", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
      _bodyEx = new Regex("<body(?<bodyattributes>.*?)>(?<body>.*?)</body>", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
      _classEx = new Regex("class.*?=.*?\"(?<bodyclass>.*?)\"", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
      ParseDocument();
    }

    public DocumentResponseData(string html, AtlantisException exAtlantis)
    {
      Html = html;
      _exception = exAtlantis;
    }

    public DocumentResponseData(string html, RequestData requestData, Exception ex)
    {
      Html = html;
      _exception = new AtlantisException(requestData, "DocumentResponseData", ex.Message, requestData.ToXML());
    }

    private void ParseDocument()
    {
      Match styleMatch = _styleEx.Match(Html);
      Styles = styleMatch.Success ? styleMatch.Value : String.Empty;

      Match bodyMatch = _bodyEx.Match(Html);

      if (bodyMatch.Success)
      {
        Body = bodyMatch.Groups[2].Captures[0].Value;
        string bodyAttributes = bodyMatch.Groups[1].Captures[0].Value;

        Match classMatch = _classEx.Match(bodyAttributes);
        BodyStyle = classMatch.Success ? classMatch.Groups[1].Captures[0].Value : String.Empty;
      }
      else
      {
        Body = Styles.Length > 0 ? Html.Replace(Styles, String.Empty) : Html;
        BodyStyle = String.Empty;
      }
    }

    public string Html { get; private set; }
    public string Styles { get; private set; }
    public string Body { get; private set; }
    public string BodyStyle { get; private set; }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      var sbResult = new StringBuilder();
      var xtwResult = new XmlTextWriter(new StringWriter(sbResult));
      xtwResult.WriteElementString("Html", Html);
      return sbResult.ToString();
    }

    #endregion
  }
}
