
namespace Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData
{
  internal class ContentData
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
    #endregion

    #region Convenience Accessor Properties
    public bool ShowOptionsButton
    {
      get { return !string.IsNullOrWhiteSpace(CiOptions); }
    }

    public bool ShowBuyLink
    {
      get { return LinkUrl != null; }
    }
    #endregion

    #region Constructors
    public ContentData(string accountList, string jsonPage, string ciOptions, LinkUrlData linkUrl)
    {
      _accountList = accountList;
      _jsonPage = jsonPage;
      _ciOptions = ciOptions;
      _linkUrl = linkUrl;
    }

    public ContentData(string accountList, string jsonPage, string ciOptions)
    {
      _accountList = accountList;
      _jsonPage = jsonPage;
      _ciOptions = ciOptions;
      _linkUrl = null;
    }
    #endregion
  }
}
