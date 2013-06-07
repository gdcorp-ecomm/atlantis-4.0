namespace Atlantis.Framework.Warmup.JIT.Interfaces
{
  using System.Collections.Generic;

  interface IAssemblyJIT
  {
    void Start(IEnumerable<string> assemblies);

    void SignalStop();

    bool IsComplete { get; }
    int Successes { get; }
    int Failures { get; }
  }
}
