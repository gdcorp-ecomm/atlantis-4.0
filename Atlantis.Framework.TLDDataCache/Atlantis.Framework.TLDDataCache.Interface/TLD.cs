using System;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class TLD : IComparable<TLD>
  {
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int DefaultOrder { get; private set; }

    public int CompareTo(TLD other)
    {
      if (other == null)
      {
        return -1;
      }
      else
      {
        return this.Id.CompareTo(other.Id);
      }
    }
  }
}
