namespace Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData
{
  public class SurveyData
  {
    #region Enums
    public enum SurveyTypes
    {
      PQC = 0,
      BV = 1
    }
    #endregion

    #region ReadOnly Properties
    private readonly SurveyTypes _type;
    public SurveyTypes Type
    {
      get { return _type; }
    }

    private readonly string _linkText;
    public string LinkText
    {
      get { return _linkText; }
    }

    private readonly string _productId;
    public string ProductId
    {
      get { return _productId; }
    }
    
    private readonly LinkUrlData _linkUrl;
    public LinkUrlData LinkUrl
    {
      get { return _linkUrl; }
    }
    #endregion

    internal SurveyData(SurveyTypes type, string linkText, string productId, LinkUrlData linkUrl)
    {
      _type = type;
      _linkText = linkText;
      _productId = productId;
      _linkUrl = linkUrl;
    }
  }
}
