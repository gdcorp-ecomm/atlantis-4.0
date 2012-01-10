using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData
{
  public class LinkUrlData
  {
    #region Enums
    private enum ServerLocationType
    {
      Undetermined = 0,
      Dev = 1,
      Test = 2,
      Ote = 3,
      Prod = 4,
    }

    public enum TypeOfLink : int
    {
      Standard = 0,
      Manager = 1
    }
    #endregion

    #region ReadOnly Properties
    private readonly string _link;
    public string Link
    {
      get { return _link; }
    }
    private readonly string _page;
    public string Page 
    { 
      get { return _page; }
    }
    private readonly TypeOfLink _type;
    public TypeOfLink Type
    {
      get { return _type; }
    }
    private readonly string _identificationRule;
    public string IdentificationRule
    {
      get { return _identificationRule; }
    }
    private readonly string _identificationValue;
    public string IdentificationValue
    {
      get { return _identificationValue; }
    }
    private readonly Dictionary<int, bool> _environmentHttpsRequirements;
    public Dictionary<int, bool> EnvironmentHttpsRequirements
    {
      get { return _environmentHttpsRequirements; }
    }
    private readonly NameValueCollection _qsKeys;
    public NameValueCollection QsKeys
    {
      get { return new NameValueCollection(_qsKeys); }
    }
    #endregion

    #region Public Methods
    public bool DoesEnvironmentRequireSecureLink(int environment)
    {
      bool isSecure = false;
      if (!EnvironmentHttpsRequirements.TryGetValue(environment, out isSecure))
      {
        isSecure = false;
      }

      return isSecure;
    }
    #endregion

    #region Constructor
    public LinkUrlData(XElement linkUrlXml)
    {
      NameValueCollection nvc = new NameValueCollection();

      if (IsWellFormedLinkUrlXml(linkUrlXml))
      {
        _link = linkUrlXml.Attribute("link").Value;
        _page = linkUrlXml.Attribute("page") != null ? linkUrlXml.Attribute("page").Value : string.Empty;
        _type = (LinkUrlData.TypeOfLink)Enum.Parse(typeof(LinkUrlData.TypeOfLink), linkUrlXml.Attribute("type").Value);
        _identificationRule = linkUrlXml.Attribute("identificationrule") != null ? linkUrlXml.Attribute("identificationrule").Value : string.Empty;
        _identificationValue = linkUrlXml.Attribute("identificationvalue") != null ? linkUrlXml.Attribute("identificationvalue").Value : string.Empty;
        _environmentHttpsRequirements = BuildEnvironmentHttpsDictionary(linkUrlXml.Attribute("isenvsecure") != null ? linkUrlXml.Attribute("isenvsecure").Value : string.Empty);

        if (linkUrlXml.HasElements)
        {
          foreach (XElement qsKey in linkUrlXml.Elements("qskey"))
          {
            nvc.Add(qsKey.Attribute("name").Value, qsKey.Attribute("value").Value);
          }
          _qsKeys = nvc;
        }
      }
    }
    #endregion

    #region Link Validation
    private XDocument LinkUrlXDoc(XElement linkUrlXml)
    {
      XDocument linkUrlXDoc;
      string msg = string.Empty;
      try
      {
        linkUrlXDoc = XDocument.Parse(linkUrlXml.ToString());
        XmlValidator.ValidateLinkXml(linkUrlXDoc, out msg);

        if (!string.IsNullOrWhiteSpace(msg))
        {
          AtlantisException aex = new AtlantisException("AccordionMetaData::LinkUrlXDoc", "0", msg, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(aex);
          linkUrlXDoc = XDocument.Parse("<linkurl error='LinkUrl Malformed'/>");
        }
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException("AccordionMetaData::LinkUrlXDoc", "0", ex.Message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);
        linkUrlXDoc = XDocument.Parse("<linkurl error='LinkUrl Malformed'/>");
      }

      return linkUrlXDoc;
    }

    private bool IsWellFormedLinkUrlXml(XElement linkUrlXml)
    {
      return !LinkUrlXDoc(linkUrlXml).Element("linkurl").FirstAttribute.Name.Equals("error");
    }
    #endregion

    #region Environment URL Handling
    private Dictionary<int, bool> BuildEnvironmentHttpsDictionary(string secureEnvironmentString)
    {
      Dictionary<int, bool> envDict = new Dictionary<int, bool>();
      List<string> envs = string.IsNullOrWhiteSpace(secureEnvironmentString) ? new List<string>() : secureEnvironmentString.Split(',').ToList<string>();

      envDict.Add((int)ServerLocationType.Dev, false);
      envDict.Add((int)ServerLocationType.Ote, false);
      envDict.Add((int)ServerLocationType.Prod, false);
      envDict.Add((int)ServerLocationType.Test, false);

      if (envs.Count > 0)
      {
        foreach (string env in envs)
        {
          envDict[(int)Enum.Parse(typeof(ServerLocationType), env)] = true;
        }
      }
      return envDict;
    }
    #endregion
  }
}
