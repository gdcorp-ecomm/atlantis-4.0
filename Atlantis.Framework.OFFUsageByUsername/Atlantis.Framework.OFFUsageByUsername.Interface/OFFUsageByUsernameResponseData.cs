using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OFFUsageByUsername.Interface
{
  public class OFFUsageByUsernameResponseData : IResponseData
  {
    public int UsagePercent { get; private set; }
    public AtlantisException AtlantisException { get; private set; }

    public OFFUsageByUsernameResponseData() { }

    public OFFUsageByUsernameResponseData(AtlantisException atlEx)
    {
      AtlantisException = atlEx;
      UsagePercent = -1;
    }

    public OFFUsageByUsernameResponseData(int usagePercent)
    {
      UsagePercent = usagePercent;
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
