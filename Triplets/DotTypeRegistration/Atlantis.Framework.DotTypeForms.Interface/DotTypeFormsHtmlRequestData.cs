namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsHtmlRequestData : DotTypeFormsBaseRequestData
  {
    public string Domain { get; set; }

    public DotTypeFormsHtmlRequestData(string formType, int tldId, string placement, string phase, string marketId, int contextId, string domain)
      : base(formType, tldId, placement, phase, marketId, contextId)
    {
      Domain = domain;
    }
  }
}
