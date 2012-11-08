using System.Collections.Generic;

namespace Atlantis.Framework.RuleEngine.Results
{
  public class FactResult : IFactResult
  {
    public FactResult(string factKey, ValidationResultStatus status)
    {
      _factKey = factKey;
      _status = status;
    }

    private readonly string _factKey;
    public string FactKey
    {
      get { return _factKey; }
    }

    public string Key { get; set; }



    private ValidationResultStatus _status = ValidationResultStatus.NotSet;
    public ValidationResultStatus Status
    {
      get { return _status; }
      set { _status = value; }
    }

    private IList<string> _messages;

    public IList<string> Messages
    {
      get
      {
        if (_messages == null)
        {
          _messages = new List<string>(0);
        }
        return _messages;
      }
      set { _messages = value; }
    }
  }
}
