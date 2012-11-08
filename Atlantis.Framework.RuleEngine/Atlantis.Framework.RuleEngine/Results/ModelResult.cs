using System.Collections.Generic;

namespace Atlantis.Framework.RuleEngine.Results
{
  public sealed class ModelResult : IModelResult
  {
    public string ModelId { get; set; }
    
    public bool ContainsInvalids { get; set; }

    private IList<IFactResult> _facts;
    public IList<IFactResult> Facts
    {
      get
      {
        if (_facts == null)
        {
          _facts = new List<IFactResult>(0);
        }

        return _facts;
      }
      set { _facts = value; }
    }
  }
}
