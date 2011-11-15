using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OFFUsageByUsername.Interface
{
  public class OFFUsageByUsernameResponseData : IResponseData
  {
    public float SpaceAvailable { get; private set; }
    public float SpaceUsed { get; private set; }
    public AtlantisException AtlantisException { get; private set; }

    public OFFUsageByUsernameResponseData() { }

    public OFFUsageByUsernameResponseData(AtlantisException atlEx)
    {
      AtlantisException = atlEx;
      SpaceAvailable = -1;
      SpaceUsed = -1;
    }

    public OFFUsageByUsernameResponseData(float spaceAvailable, float spaceUsed)
    {
      SpaceAvailable = spaceAvailable;
      SpaceUsed = spaceUsed;
    }
    
    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return AtlantisException;
    }
  }
}
