
namespace Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData
{
  internal class WorkspaceLoginData
  {
    #region ReadOnly Properties
    private readonly LinkUrlData _linkUrl;
    public LinkUrlData LinkUrl
    {
      get { return _linkUrl; }
    }
    private readonly string _buttonText;
    public string ButtonText
    {
      get { return _buttonText; }
    }
    #endregion

    #region Convenience Accessor Properties
    public bool HasLink
    {
      get { return LinkUrl != null; }
    }
    #endregion

    public WorkspaceLoginData(LinkUrlData linkUrl, string buttonText)
    {
      _linkUrl = linkUrl;
      _buttonText = buttonText;
    }
  }
}
