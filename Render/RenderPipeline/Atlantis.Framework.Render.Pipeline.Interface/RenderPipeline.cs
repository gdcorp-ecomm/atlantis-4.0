﻿using System.Collections.Generic;

namespace Atlantis.Framework.Render.Pipeline.Interface
{
  internal class RenderPipeline
  {
    private readonly IList<IRenderHandler> _renderHandlers = new List<IRenderHandler>(128);
 
    internal IList<IRenderHandler> RenderHandlers { get { return _renderHandlers; } }

    internal void Add(IRenderHandler renderHandler)
    {
      _renderHandlers.Add(renderHandler);
    }
  }
}
