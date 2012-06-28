
namespace Atlantis.Framework.MktgSubscribeGet.Interface
{
  public class MktgSubscribeOptIn
  {
    public int Id { get; private set; }

    public string Description { get; private set; }

    public MktgSubscribeOptIn(int id, string description)
    {
      Id = id;
      Description = description;
    }
  }
}
