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

    private string _messages;
    public string Message
    {
      get
      {
        if (_messages == null)
        {
          _messages = string.Empty;
        }
        return _messages;
      }
      set { _messages = value; }
    }
  }
}
