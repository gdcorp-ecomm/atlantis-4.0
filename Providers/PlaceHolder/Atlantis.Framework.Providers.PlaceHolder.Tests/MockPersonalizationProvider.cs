using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;
using Atlantis.Framework.Providers.Personalization;
using Atlantis.Framework.Providers.PlaceHolder.Tests.Properties;


namespace Atlantis.Framework.Providers.PlaceHolder.Tests
{
  [ExcludeFromCodeCoverage]
  public class MockPersonalizationProvider : PersonalizationProvider
  {
    public MockPersonalizationProvider(IProviderContainer container)
      : base(container)
    {
    }

    public override TargetedMessages GetTargetedMessages(string interactionName)
    {

      if (interactionName == "explode")
      {
        throw new Exception("exploding");
      }

      TargetedMessages messages = null;
  
      try
      {
        var targetMessageXml = XDocument.Parse(Resources.MockTMSData);

        if (targetMessageXml.Root != null)
        {
          using (XmlReader reader = targetMessageXml.Root.CreateReader())
          {
            XmlSerializer serializer = new XmlSerializer(typeof(TargetedMessages), targetMessageXml.Root.GetDefaultNamespace().NamespaceName);
            messages = (TargetedMessages)serializer.Deserialize(reader);
            reader.Close();
          }
        }
      }
      catch (Exception ex)
      {
        ErrorLogger.LogException(ex.Message, "MockPersonalizationProvider", string.Empty);
      }

      return messages;
    }
  }
}
