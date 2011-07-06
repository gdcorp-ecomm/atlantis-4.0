using System.Collections.Generic;

namespace Atlantis.Framework.Interface.Tests
{
  public interface IEmployeeProvider : IProviderContainer
  {
    IList<string> Employees { get; }
  }
}
