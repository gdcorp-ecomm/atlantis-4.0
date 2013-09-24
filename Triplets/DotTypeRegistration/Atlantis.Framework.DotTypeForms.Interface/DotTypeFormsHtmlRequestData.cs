namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsHtmlRequestData : DotTypeFormsBaseRequestData
  {
    public DotTypeFormsHtmlRequestData(string formType, int tldId, string placement, string phase, string marketId, int contextId)
      : base(formType, tldId, placement, phase, marketId, contextId)
    {
    }
  }
}
