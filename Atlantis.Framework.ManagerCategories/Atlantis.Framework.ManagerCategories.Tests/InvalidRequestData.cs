using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.ManagerCategories.Tests
{
  internal class InvalidRequestData : RequestData
  {
    public InvalidRequestData() : base(string.Empty, string.Empty, string.Empty, string.Empty, 0)
    {

    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
