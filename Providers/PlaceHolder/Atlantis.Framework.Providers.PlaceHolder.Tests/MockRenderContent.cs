using Atlantis.Framework.Providers.RenderPipeline.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  public class MockRenderContent : IRenderContent
  {
    public string Content { get; set; }
  }
}