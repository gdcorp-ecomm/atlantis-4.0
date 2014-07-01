using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Tokens.Interface;
using System.Web.Configuration;

namespace Atlantis.Framework.TH.Links
{
  public class LinkToken : XmlToken
  {
    public string RenderType
    {
      get { return (TokenData != null) ? TokenData.Name.ToString() : string.Empty; }
    }

    public string LinkType { get; private set; }
    public QueryParamMode ParamMode { get; private set; }
    public string Path { get; private set; }
    public string NameMode { get; private set; }
    public bool? Secure { get; private set; }
    public NameValueCollection Params { get; private set; }
    public bool? IncludeQueryStringParams { get; private set; }

    public LinkToken(string key, string data, string fullTokenString)
      : base(key, data, fullTokenString)
    {
      LinkType = GetAttributeText("linktype", string.Empty);
      
      string textParamMode = GetAttributeText("parammode", "common");

      ParamMode = textParamMode == "explicit" ? QueryParamMode.ExplicitParameters : QueryParamMode.CommonParameters;

      Path = GetAttributeText("path", string.Empty);

      NameMode = GetAttributeText("namemode", string.Empty);

      if (TokenData != null)
      {
        if (TokenData.Attributes("secure").Any())
        {
          Secure = GetAttributeBool("secure", false);
        }
        else
        {
          Secure = null;
        }

        if (TokenData.HasElements && TokenData.Elements("param").Any())
        {
          Params = new NameValueCollection();

          IEnumerable<XElement> tokenParams = TokenData.Elements("param");
          foreach (XElement tokenParam in tokenParams)
          {
            string name = (tokenParam.Attribute("name") != null) ? tokenParam.Attribute("name").Value : null;
            string value = (tokenParam.Attribute("value") != null) ? tokenParam.Attribute("value").Value : null;

            if (!string.IsNullOrEmpty(name) && value != null)
            {
              Params[name] = value;
            }
          }
        }

        IncludeQueryStringParams = TokenData.Attributes("includequery").Any() && GetAttributeBool("includequery", false);

        if (IncludeQueryStringParams == true && ParamMode == QueryParamMode.CommonParameters)
        {
          NameValueCollection queryStrings = HttpContext.Current.Request.QueryString;

          if (Params == null)
          {
            Params = new NameValueCollection();
          }

          foreach (string qsKey in queryStrings)
          {
            if ((qsKey != "ci") && (string.IsNullOrEmpty(Params[qsKey])))
            {
              Params[qsKey] = queryStrings[qsKey];
            }
          }
        }
        
      }
    }
  }
}
