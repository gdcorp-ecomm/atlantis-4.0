using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.TMSContent.Interface;


namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  [ExcludeFromCodeCoverage]
  public class MockTMSContentProvider : ProviderBase, ITMSContentProvider
  {
    public MockTMSContentProvider(IProviderContainer container)
      : base(container) {}

    #region ITMSContentProvider Members

    public void ConsumeMessageVariant(string appProduct, string interactionPoint, MessageVariant message) { }

    public string GetTrackingData() { return null; }

    public bool TryGetNextMessageVariant(string appProduct, string interactionName, out MessageVariant messageVariant)
    {
      messageVariant = null;

      if (interactionName == "explode")
      {
        throw new Exception("exploding");
      }

      if (interactionName == "First")
      {
        messageVariant = new MessageVariant
        {
          Id = "ID1",
          Tag = "FirstTag",
          Name = "First",
          TrackingId = "1",
          HasContent = true
        };
        return true;
      }
      if (interactionName == "Second")
      {
        messageVariant = new MessageVariant
        {
          Id = "ID2",
          Tag = "SecondTag",
          Name = "Second",
          TrackingId = "2",
          HasContent = true
        };
        return true;
      }
      if (interactionName == "NoContent")
      {
        messageVariant = new MessageVariant
        {
          Id = "ID1",
          Tag = "NoContentTag",
          Name = "NoContent",
          TrackingId = "1",
          HasContent = false
        };
        return true;
      }

      return false;
    }

    public string TrackingData { get; private set; }

    #endregion
  }
}
