using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;

using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Tokens.Interface;

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
    public bool? Secure { get; private set; }
    public NameValueCollection Params { get; private set; }

    public LinkToken(string key, string data, string fullTokenString)
      : base(key, data, fullTokenString)
    {
      LinkType = this.GetAttributeText("linktype", string.Empty);
      
      string textParamMode = this.GetAttributeText("parammode", "common");
      if (textParamMode == "explicit")
      {
        ParamMode = QueryParamMode.ExplicitParameters;
      }
      else
      {
        ParamMode = QueryParamMode.CommonParameters;
      }

      Path = this.GetAttributeText("path", string.Empty);

      if (TokenData.Attributes("secure").Any())
      {
        Secure = this.GetAttributeBool("secure", false);
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
          string name = (tokenParam.Attribute("name") != null ) ? tokenParam.Attribute("name").Value : null;
          string value = (tokenParam.Attribute("value") != null ) ? tokenParam.Attribute("value").Value : null;

          if (!string.IsNullOrEmpty(name) && value != null)
          {
            Params[name] = value;
          }
        }
      }
    }
  }
}
