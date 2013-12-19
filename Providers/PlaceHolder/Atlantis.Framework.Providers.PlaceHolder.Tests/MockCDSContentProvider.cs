using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  public class MockCDSContentProvider : ProviderBase, ICDSContentProvider
  {
    public MockCDSContentProvider(IProviderContainer container)
      : base(container)
    {
    }

    public IRedirectResult CheckRedirectRules(string appName, string relativePath)
    {
      throw new NotImplementedException();
    }

    public CDS.Interface.IWhitelistResult CheckWhiteList(string appName, string relativePath)
    {
      throw new NotImplementedException();
    }

    public RenderPipeline.Interface.IRenderContent GetContent(string appName, string relativePath)
    {
      MockRenderContent render = new MockRenderContent();
      render.Content = "[@D[TMSMessageId]@D] [@D[TMSMessageTag]@D] [@D[TMSMessageName]@D] [@D[TMSMessageTrackingId]@D]";
      return render;
    }
  }
}