using System.Collections.Generic;

namespace Atlantis.Framework.RuleEngine.Results
{
  public interface IModelResult
  {
    string ModelId { get; set; }
    bool ContainsInvalids { get; set; }
    IList<IFactResult> Facts { get; set; }
  }
}
