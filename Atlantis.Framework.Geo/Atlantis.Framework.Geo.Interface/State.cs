using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Interface
{
  public class State
  {
    public int Id { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }

    private State() { }

    public static State FromCacheElement(XElement stateElement)
    {
      State result = new State();

      result.Id = stateElement.GetAttributeValueInt("id", 0);
      result.Code = stateElement.GetAttributeValue("code", string.Empty);
      result.Name = stateElement.GetAttributeValue("name", string.Empty);

      return result;
    }
  }
}
