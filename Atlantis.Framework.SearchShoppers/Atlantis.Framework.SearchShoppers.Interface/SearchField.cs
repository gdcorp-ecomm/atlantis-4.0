
namespace Atlantis.Framework.SearchShoppers.Interface
{
  public class SearchField
  {
    public SearchField(string name, string searchValue)
    {
      FieldName = name;
      MatchValue = searchValue;
    }

    public string FieldName { get; private set; }
    public string MatchValue { get; private set; }
  }
}
