using System.Collections.Generic;
using System.Collections.Specialized;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData
{
  public class LinkUrlData
  {
    #region Enums

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
    public NameValueCollection GetNewQsKeys()
    {
      return new NameValueCollection(_qsKeys);
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

    internal LinkUrlData(string link, string page, TypeOfLink type, string identificationRule, string identificationValue, Dictionary<int, bool> envHttpsRequirements, NameValueCollection qsKeys)
    {
      _link = link;
      _page = page;
      _type = type;
      _identificationRule = identificationRule;
      _identificationValue = identificationValue;
      _environmentHttpsRequirements = envHttpsRequirements;
      _qsKeys = qsKeys;
    }

    internal LinkUrlData() {}

  }
}
