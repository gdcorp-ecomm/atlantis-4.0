using System.Collections.Generic;
using System.ComponentModel.Composition;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Conditions.Interface
{
  [InheritedExport]
  public interface IConditionHandler
  {
    string ConditionName { get; }

    bool EvaluateCondition(string conditionName, IEnumerable<string> parameters, IProviderContainer providerContainer);
  }
}