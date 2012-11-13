using System.Collections.Generic;

namespace Atlantis.Framework.RuleEngine.Results
{
  public interface IFactResult
  {
    string FactKey { get; }
    ValidationResultStatus Status { get; set; }
    string Message { get; set; } 
  }
}
