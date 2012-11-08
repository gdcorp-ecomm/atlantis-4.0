using System.Collections.Generic;

namespace Atlantis.Framework.RuleEngine.Results
{
  public interface IRuleEngineResult
  {
    IList<IModelResult> ValidationResults { get; }
    RuleEngineResultStatus Status { get; }
  }
}
