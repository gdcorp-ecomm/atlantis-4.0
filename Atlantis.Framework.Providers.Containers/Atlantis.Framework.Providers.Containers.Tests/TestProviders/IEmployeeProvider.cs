using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Containers.Tests
{
  public interface IEmployeeProvider : IProviderContainer
  {
    IList<string> Employees { get; }
  }
}
