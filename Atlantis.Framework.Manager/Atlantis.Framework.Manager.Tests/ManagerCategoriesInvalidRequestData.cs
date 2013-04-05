using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Manager.Tests
{
  internal class ManagerCategoriesInvalidRequestData : RequestData
  {
    public ManagerCategoriesInvalidRequestData()
      : base(string.Empty, string.Empty, string.Empty, string.Empty, 0)
    {

    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
