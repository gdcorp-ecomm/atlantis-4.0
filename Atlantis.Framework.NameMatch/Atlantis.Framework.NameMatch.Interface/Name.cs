
namespace Atlantis.Framework.NameMatch.Interface
{
  public class Name
  {
    private string _nameWithoutExtension;
    public string NameWithoutExtension
    {
      get
      {
        return this._nameWithoutExtension;
      }
      set
      {
        this._nameWithoutExtension = value;
      }
    }

    private string[] _keys;
    public string[] Keys
    {
      get
      {
        return this._keys;
      }
      set
      {
        this._keys = value;
      }
    }

    private DomainData[][] _data;
    public DomainData[][] Data
    {
      get
      {
        return this._data;
      }
      set
      {
        this._data = value;
      }
    }
  }
}
