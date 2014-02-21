using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.RenderPipeline.Interface
{
  public interface IRenderPipelineStatus
  {
    RenderPipelineResult Status { get; set; }

    string Source { get; }

    IEnumerable<AtlantisException> Exceptions { get; }

    IEnumerable<KeyValuePair<string, string>> Data { get; }

    void AddException(AtlantisException exception);

    void AddData(string key, string value);
  }
}
