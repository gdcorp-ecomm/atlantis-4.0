using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.RuleEngine.Evidence.EvidenceValue;

namespace Atlantis.Framework.RuleEngine.Evidence.Expression
{
  public struct Symbol
  {
    public string Name;
    public IEvidenceValue Value;
    public ExpressionEvaluator.Type SymbolType;
    public override string ToString()
    {
      return Name;
    }

    public string ConditionKey { get; set; }
  }
}
