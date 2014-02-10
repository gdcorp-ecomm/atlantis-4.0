using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public abstract class DotTypeFormsBaseRequestData : RequestData
  {
    public int TldId { get; set; }
    public string Placement { get; set; }
    public string Phase { get; set; }
    public string MarketId { get; set; }
    public int ContextId { get; set; }
    public string FormType { get; set; }

    protected DotTypeFormsBaseRequestData(string formType, int tldId, string placement, string phase, string marketId, int contextId)
    {
      FormType = formType;
      TldId = tldId;
      Placement = placement.ToLowerInvariant();
      Phase = phase.ToLowerInvariant();
      MarketId = marketId.ToLowerInvariant();
      ContextId = contextId;
    }
  }
}
