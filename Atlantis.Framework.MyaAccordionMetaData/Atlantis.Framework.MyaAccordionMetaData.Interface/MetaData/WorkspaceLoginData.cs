
namespace Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData
{
  public class WorkspaceLoginData
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
    private bool _hasLink;
    public bool HasLink
    {
      get { return _hasLink; }
    }
    #endregion

    internal WorkspaceLoginData(LinkUrlData linkUrl, string buttonText)
    {
      _linkUrl = linkUrl;
      _buttonText = buttonText;
      _hasLink = LinkUrl != null;
    }
  }
}
