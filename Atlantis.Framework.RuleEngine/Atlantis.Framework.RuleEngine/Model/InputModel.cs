using System.Collections.Generic;

namespace Atlantis.Framework.RuleEngine.Model
{
  public sealed class InputModel
  {
    public InputModel(string modelId, Dictionary<string, string> models)
    {
      _modelId = modelId;
      _models = models;
    }

    private readonly string _modelId;
    public string ModelId
    {
      get { return _modelId; }
    }

    private readonly Dictionary<string, string> _models;
    public Dictionary<string, string> Models
    {
      get { return _models; }
    }
  }
}
