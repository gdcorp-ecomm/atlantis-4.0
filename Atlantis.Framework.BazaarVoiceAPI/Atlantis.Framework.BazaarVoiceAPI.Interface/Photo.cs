using System.Collections.Generic;

namespace Atlantis.Framework.BazaarVoiceAPI.Interface
{
  public class Photo
  {
    public string Id { get; internal set; }
    public List<Size> Sizes { get; internal set; }
  }
}
