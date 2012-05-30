
namespace Atlantis.Framework.AffiliateMetaData.Interface
{
  public class AffiliateData
  {
    private readonly string _prefix;
    public string Prefix
    {
      get { return _prefix; }
    }

    private readonly string _privateLabelId;
    public string PrivateLabelId
    {
      get { return _privateLabelId; }
    }

    public bool IsGlobalPrefix
    {
      get { return _privateLabelId.Equals("0"); }
    }

    public AffiliateData(string prefix, string privateLabelId)
    {
      _prefix = prefix;
      _privateLabelId = privateLabelId;
    }
  }
}
