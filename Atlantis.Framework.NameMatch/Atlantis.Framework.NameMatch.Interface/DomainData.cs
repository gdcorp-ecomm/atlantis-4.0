
namespace Atlantis.Framework.NameMatch.Interface
{
  public class DomainData
  {
    private string _name;
    public string Name
    {
      get
      {
        return this._name;
      }
      set
      {
        this._name = value;
      }
    }

    private object _data;
    public object Data
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
