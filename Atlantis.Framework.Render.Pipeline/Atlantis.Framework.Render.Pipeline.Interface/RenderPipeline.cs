using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Render.Pipeline.Interface
{
  internal class RenderPipeline
  {
    private readonly IList<IRenderHandler> _renderHandlers = new List<IRenderHandler>(128);
    private readonly HashSet<string> _renderHandlerTypeHastSet = new HashSet<string>();
 
    internal IList<IRenderHandler> RenderHandlers { get { return _renderHandlers; } }

    private void LogException(string sourceMethod, string message, string data)
    {
      AtlantisException aex = new AtlantisException(sourceMethod,
                                                    "0",
                                                    message,
                                                    data,
                                                    null,
                                                    null);

      Engine.Engine.LogAtlantisException(aex);
    }

    internal void Add(IRenderHandler renderHandler)
    {
      string type = renderHandler.GetType().ToString();

      if (_renderHandlerTypeHastSet.Contains(type))
      {
        LogException("RenderPipeline.Add()", 
                     string.Format("Attempted to add duplicate IRenderHandler. Type: \"{0}\"", type), 
                     type);
      }
      else
      {
        _renderHandlers.Add(renderHandler);
        _renderHandlerTypeHastSet.Add(type);
      }
    }
  }
}
