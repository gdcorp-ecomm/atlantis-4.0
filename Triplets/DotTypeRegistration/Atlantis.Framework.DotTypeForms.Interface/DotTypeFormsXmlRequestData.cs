namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsXmlRequestData : DotTypeFormsBaseRequestData
  {
    public DotTypeFormsXmlRequestData(string formType, int tldId, string placement, string phase, string marketId, int contextId)
      : base(formType, tldId, placement, phase, marketId, contextId)
    {
    }
  }
}
