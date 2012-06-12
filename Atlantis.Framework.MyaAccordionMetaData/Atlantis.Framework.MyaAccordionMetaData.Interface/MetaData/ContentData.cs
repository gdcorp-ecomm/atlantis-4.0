﻿
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
    private readonly LinkUrlData _linkUrl;
    public LinkUrlData LinkUrl
    {
      get { return _linkUrl; }
    }
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
    private readonly bool _showOptionsButtonForManagerOnly;
    public bool ShowOptionsButtonForManagerOnly
    {
      get { return _showOptionsButtonForManagerOnly; }
    }
    #endregion

    #region Constructors
    internal ContentData(string accountList, string jsonPage, string ciOptions, string optionsMgrOnly, LinkUrlData linkUrl)
    {
      _accountList = accountList;
      _jsonPage = jsonPage;
      _ciOptions = ciOptions;
      _linkUrl = linkUrl;
      _showOptionsButton = !string.IsNullOrEmpty(CiOptions);
      _showBuyLink = LinkUrl != null;
      _showOptionsButtonForManagerOnly = !string.IsNullOrEmpty(optionsMgrOnly);
    }

    internal ContentData(string accountList, string jsonPage, string ciOptions)
      : this(accountList, jsonPage, ciOptions, null, null)
    { }

    internal ContentData(string accountList, string jsonPage, string ciOptions, string optionsMgrOnly)
      : this(accountList, jsonPage, ciOptions, optionsMgrOnly, null)
    { }

    internal ContentData(string accountList, string jsonPage, string ciOptions, LinkUrlData linkUrl)
      : this(accountList, jsonPage, ciOptions, null, linkUrl)
    { }
    #endregion
  }
}
