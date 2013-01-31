using System.Collections.Generic;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData
{
  public class ContentData
  {
    #region ReadOnly Properites
    private readonly string _accountList;
    public string AccountList
    {
      get { return _accountList; }
    }
    private readonly string _jsonPage;
    public string JsonPage
    {
      get { return _jsonPage; }
    }
    private readonly string _ciOptions;
    public string CiOptions
    {
      get { return _ciOptions; }
    }
    private readonly LinkUrlData _buyMoreLinkUrl;
    public LinkUrlData BuyMoreLink
    {
      get { return _buyMoreLinkUrl; }
    }
    private readonly LinkUrlData _helpLinkUrl;
    public LinkUrlData HelpLink
    {
      get { return _helpLinkUrl; }
    }
    private readonly List<LinkUrlData> _links;
    private readonly bool _showOptionsButton;
    public bool ShowOptionsButton
    {
      get { return _showOptionsButton; }
    }
    private readonly bool _showBuyLink;
    public bool ShowBuyLink
    {
      get { return _showBuyLink; }
    }
    private readonly bool _showHelpLink;
    public bool ShowHelpLink
    {
      get { return _showHelpLink; }
    }
    private readonly bool _showOptionsButtonForManagerOnly;
    public bool ShowOptionsButtonForManagerOnly
    {
      get { return _showOptionsButtonForManagerOnly; }
    }
    #endregion

    #region Constructors
    internal ContentData(string accountList, string jsonPage, string ciOptions, string optionsMgrOnly=null, List<LinkUrlData> links=null)
    {
      _accountList = accountList;
      _jsonPage = jsonPage;
      _ciOptions = ciOptions;
      _links = links;
      _buyMoreLinkUrl = SetBuyMoreLink();
      _showOptionsButton = !string.IsNullOrEmpty(CiOptions);
      _showBuyLink = BuyMoreLink != null;
      _helpLinkUrl = SetHelpLink();
      _showHelpLink = HelpLink != null;
      _showOptionsButtonForManagerOnly = !string.IsNullOrEmpty(optionsMgrOnly);
    }
    #endregion

    private LinkUrlData SetHelpLink()
    {
      LinkUrlData helpLink = null;

      if (_links != null)
      {
        helpLink = _links.Find(link => link.IdentificationValue.Equals("Help"));
      }

      return helpLink;
    }

    private LinkUrlData SetBuyMoreLink()
    {
      LinkUrlData bmLink = null;

      if (_links != null)
      {
        bmLink = _links.Find(link => link.IdentificationValue.Equals("Buy"));
      }

      return bmLink;     
    }

  }
}
