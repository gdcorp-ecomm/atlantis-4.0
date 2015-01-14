using System;
using System.Linq;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  public class MockCDSContentProvider : ProviderBase, ICDSContentProvider
  {
    public MockCDSContentProvider(IProviderContainer container)
      : base(container) {}

    public IRedirectResult CheckRedirectRules(string appName, string relativePath)
    {
      throw new NotImplementedException();
    }

    public IWhitelistResult CheckWhiteList(string appName, string relativePath)
    {
      throw new NotImplementedException();
    }

    public IRenderContent GetContent(string appName, string relativePath)
    {
      MockRenderContent render = new MockRenderContent();
      switch (relativePath.ToLower())
      {
        case "dev/test/home-missing":
          render.Content =
            "<div>" +
              "<h1>dev/test/home-missing</h1>" +
              "<ul>" +
                "<li>ID: [@D[tms.message.message_id]@D]</li>" +
                "<li>Tag: [@D[tms.message.tag]@D]</li>" +
                "<li>Name: [@D[tms.message.name]@D]</li>" +
                "<li>Strategy: [@D[tms.message.strategy]@D]</li>" +
                "<li>TrackingID: [@D[tms.message.tracking_id]@D]</li>" +
                "<li>Data[1]: [@D[tms.message.data.key1]@D]</li>" +
                "<li>Data[2]: [@D[tms.message.data.key2]@D]</li>" +
              "</ul>" +
            "</div>";
          break;

        case "dev/test/home":
          render.Content =
            "<div>" +
              "<h1>dev/test/home</h1>" +
              "<ul>" +
                "<li>ID: [@D[tms.message.message_id]@D]</li>" +
                "<li>Tag: [@D[tms.message.tag]@D]</li>" +
                "<li>Name: [@D[tms.message.name]@D]</li>" +
                "<li>Strategy: [@D[tms.message.strategy]@D]</li>" +
                "<li>TrackingID: [@D[tms.message.tracking_id]@D]</li>" +
                "<li>Data[1]: [@D[tms.message.data.key1]@D]</li>" +
                "<li>Data[2]: [@D[tms.message.data.key2]@D]</li>" +
              "</ul>" +
            "</div>";
          break;

        case "dev/test/message1/home":
          render.Content =
            "<div>" +
              "<h1>dev/test/message1/home</h1>" +
              "<ul>" +
                "<li>ID: [@D[tms.message.message_id]@D]</li>" +
                "<li>Tag: [@D[tms.message.tag]@D]</li>" +
                "<li>Name: [@D[tms.message.name]@D]</li>" +
                "<li>Strategy: [@D[tms.message.strategy]@D]</li>" +
                "<li>TrackingID: [@D[tms.message.tracking_id]@D]</li>" +
                "<li>Data[1]: [@D[tms.message.data.key1]@D]</li>" +
                "<li>Data[2]: [@D[tms.message.data.key2]@D]</li>" +
              "</ul>" +
            "</div>";
          break;

        case "dev/test/message2/home":
          render.Content =
            "<div>" +
              "<h1>dev/test/message2/home</h1>" +
              "<ul>" +
                "<li>ID: [@D[tms.message.message_id]@D]</li>" +
                "<li>Tag: [@D[tms.message.tag]@D]</li>" +
                "<li>Name: [@D[tms.message.name]@D]</li>" +
                "<li>Strategy: [@D[tms.message.strategy]@D]</li>" +
                "<li>TrackingID: [@D[tms.message.tracking_id]@D]</li>" +
                "<li>Data[1]: [@D[tms.message.data.key1]@D]</li>" +
                "<li>Data[2]: [@D[tms.message.data.key2]@D]</li>" +
              "</ul>" +
            "</div>";
          break;
      }
      return render;
    }
  }
}
