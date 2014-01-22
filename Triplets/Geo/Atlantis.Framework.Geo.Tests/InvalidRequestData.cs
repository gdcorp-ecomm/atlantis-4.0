using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Geo.Tests
{
  internal class InvalidRequestData : RequestData
  {
    public InvalidRequestData() { }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
