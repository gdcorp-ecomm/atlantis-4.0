using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Providers.RenderPipeline
{
  public class RenderPipelineStatus : IRenderPipelineStatus
  {
    internal List<AtlantisException> _exceptions;

    internal List<KeyValuePair<string, string>> _data; 

    
    
    public RenderPipelineResult Status { get; set; }

    public string Source { get; private set; }



    internal RenderPipelineStatus(RenderPipelineResult status, string source, IEnumerable<AtlantisException> exceptions = null, IEnumerable<KeyValuePair<string, string>> data = null)
    {
      Status = status;
      Source = source;

      if (exceptions != null)
      {
        _exceptions = new List<AtlantisException>(exceptions);
      }
      else
      {
        _exceptions = new List<AtlantisException>();
      }

      if (data != null)
      {
        _data = new List<KeyValuePair<string, string>>(data);
      }
      else
      {
        _data = new List<KeyValuePair<string, string>>();
      }
    }



    public IEnumerable<AtlantisException> Exceptions
    {
      get { return _exceptions; }
    }

    public IEnumerable<KeyValuePair<string, string>> Data
    {
      get { return _data; }
    }

    public void AddException(AtlantisException exception)
    {
      _exceptions.Add(exception);
    }

    public void AddData(string key, string value)
    {
      _data.Add(new KeyValuePair<string, string>(key, value));
    }
  }
}
